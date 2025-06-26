namespace PatientService;

public record PatientAddCommand(PatientAddRequestDto requestDto) : IRequest<Response<PatientAddResponseDto>>{}
public sealed class PatientAddCommandHandler : IRequestHandler<PatientAddCommand, Response<PatientAddResponseDto>>
{
    private readonly IMongoRepository<Patient> _repo;


    public PatientAddCommandHandler(
        IMongoRepository<Patient> repo)
    {
        _repo = repo;
    }


    // Step1: Check if Patient already exists
    // Step2: if exists return success
    // Step3: if not, Save new Patient
    // Step4: return success

    public async Task<Response<PatientAddResponseDto>> Handle(PatientAddCommand request, CancellationToken cancellationToken)
    {
        await _repo.Create(request.requestDto.New());

        // Return Success
        return new PatientAddResponseDto("Success");
    }



}



