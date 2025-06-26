namespace ClinicService;

public record ClinicDoctorAddCommand(ClinicDoctorAddRequestDto requestDto) : IRequest<Response<ClinicDoctorAddResponseDto>>{}
public sealed class ClinicDoctorAddCommandHandler : IRequestHandler<ClinicDoctorAddCommand, Response<ClinicDoctorAddResponseDto>>
{
    private readonly IClinicDoctorAddRepository _repo;
    private readonly IUnitOfWork _unitOfWork;



    public ClinicDoctorAddCommandHandler(
        IClinicDoctorAddRepository repo,
        IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }


    // Step1: Check if Clinic Doctor already exists
    // Step2: if exists return success
    // Step3: if not, Save new ClinicDoctor
    // Step4: return success

    public async Task<Response<ClinicDoctorAddResponseDto>> Handle(ClinicDoctorAddCommand request, CancellationToken cancellationToken)
    {
        // Check if ClinicDoctor already exists
        DoctorClinic ClinicDoctor = await _repo.Get(
            t => t.DoctorId == request.requestDto.DoctorId &&
            t.ClinicId == request.requestDto.ClinicId &&
            t.IsActive == true
        );

        // if exists return success
        if (ClinicDoctor is not null)
            return new ClinicDoctorAddResponseDto("Success");


        // Save new ClinicDoctor
        await _repo.Add(request.requestDto.New());


        // Commit
        var commitResult = await _unitOfWork.SaveChangesAsync();
        if (commitResult.IsFailure)
            return commitResult.Error;

        // Return Success
        return new ClinicDoctorAddResponseDto("Success");
    }



}



