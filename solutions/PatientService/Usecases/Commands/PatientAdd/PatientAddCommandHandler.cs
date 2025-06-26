using GPlay.Auth.Services;

namespace PatientService;

public record PatientAddCommand(PatientAddRequestDto requestDto) : IRequest<Response<PatientAddResponseDto>>{}
public sealed class PatientAddCommandHandler(IHybridCacheService _hybridCache, IMongoRepository<Patient>  _repo) : IRequestHandler<PatientAddCommand, Response<PatientAddResponseDto>>
{



    // Step1: Get patient unique cache key using mobile
    // Step2: Check if Patient already exists in cache
    // Step3: if exists return Patient already exists
    // Step4: if not, check if Patient exists in DB
    // Step5: if exists return Patient already exists
    // Step6: if not, create new Patient object
    // Step7: Save new Patient to cache
    // Step8: return success

    public async Task<Response<PatientAddResponseDto>> Handle(PatientAddCommand request, CancellationToken cancellationToken)
    {

        // Get patient unique cache key using mobile
        var mobile = request.requestDto.ContactInfo.Phone;
        var newPatientCacheKey = RedisKeys.GetNewPatientKey(mobile);
        var patientCacheKey = RedisKeys.GetPatientMobileKey(mobile);

        // Check if Patient already exists in cache
        var existingPatientInCache = await _hybridCache.GetOrCreateAsync<Patient>(newPatientCacheKey, async entry => null);

        // if exists return Patient already exists
        if (existingPatientInCache.IsSuccess && existingPatientInCache.Value is not null)
            return new PatientAddResponseDto("Patient already exists");


        // Check if Patient already exists in DB
        var existingPatientInDB = await HasPatientWithMobile(mobile);

        // if exists return Patient already exists
        if (existingPatientInDB is true)
            return new PatientAddResponseDto("Patient already exists");


        // Create new Patient object
        var newPatient = request.requestDto.New();

        // Save new Patient to cache
        await _hybridCache.SetAsync(
            newPatientCacheKey,
            newPatient,
            TimeSpan.FromHours(24)
        );

        // For Other Service usages
        await _hybridCache.SetAsync(
            patientCacheKey,
            newPatient,
            TimeSpan.FromHours(24)
        );


        // Return Success
        return new PatientAddResponseDto("Success");
    }
    
    public async Task<Boolean> HasPatientWithMobile(string Mobile) =>
        (await _repo.Get(x => x.ContactInfo.Phone == Mobile)) is not null ? true : false;



}



