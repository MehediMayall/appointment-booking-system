using Quartz;
using StackExchange.Redis;

namespace ClinicService;


// This job is to Load all available slots from database in every 1 minute
[DisallowConcurrentExecution]
public sealed class LoadAvailableSlotsIntoCacheJob(IGetDoctorAvailableSlotsRepository  _repo, IHybridCacheService _hybridCache) : IJob
{


    // Step1 : Get all available lots From Database
    // Step2 : // Save to HybridCache  
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            // Get all available lots
            var availableSlots = await _repo.GetAvailableSlots();

            // Save to HybridCache
            await _hybridCache.SetAsync(
                RedisKeys.GetAvailableSlotsKey(),
                availableSlots
            );

        }
        catch (Exception ex) { Log.Error(ex.GetAllExceptions()); }
    }
    
   
}

