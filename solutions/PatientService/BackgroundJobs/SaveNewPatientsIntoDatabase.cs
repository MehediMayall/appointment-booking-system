using Quartz;
using StackExchange.Redis;

namespace PatientService;


// This job is to save new patients into database in every 30 seconds
[DisallowConcurrentExecution]
public sealed class SaveNewPatientsIntoDatabase(IMongoRepository<Patient>  _repo, IHybridCacheService _hybridCache) : IJob
{


    // Step1 : Connect to Redis
    // Step2 : Get all new patient keys from redis
    // Step3 : Get patients from cache and add to Patient List
    // Step4 : Save to DB    
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            // Connect to Redis
            var redis = ConnectionMultiplexer.Connect("localhost");
            IDatabase db = redis.GetDatabase();
            var server = redis.GetServer(redis.GetEndPoints()[0]);

            // Get all new patient keys from redis
            var keys = server.Keys(pattern: "newpatient:*");
            var newPatiensKeys = keys.Select(k => k.ToString()).ToList();

            // Do nothing if no keys
            if (newPatiensKeys.Count == 0)
                return;

            List<Patient> patients = new List<Patient>();
            List<string> keysToRemove = new List<string>();

            // Get patients from cache and add to Patient List
            foreach (var key in newPatiensKeys)
            {
                var patientResult = await _hybridCache.GetOrCreateAsync<Patient>(key, async entry => null);
                if (patientResult.IsFailure)
                    continue;

                if (patientResult.IsSuccess && patientResult.Value is null)
                {
                    await _hybridCache.RemoveAsync(key);
                    continue;
                }

                patients.Add(patientResult.Value);
                keysToRemove.Add(key);
            }

            // Save to DB 
            await _repo.CreateMany(patients);

            // Remove from cache
            foreach (var key in keysToRemove)
                await _hybridCache.RemoveAsync(key);

        }
        catch (Exception ex) { Log.Error(ex.GetAllExceptions()); }
    }
    
   
}

