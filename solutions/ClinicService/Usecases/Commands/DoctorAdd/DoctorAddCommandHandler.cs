namespace ClinicService;

public record DoctorAddCommand(DoctorAddRequestDto requestDto) : IRequest<Response<DoctorAddResponseDto>>{}
public sealed class DoctorAddCommandHandler : IRequestHandler<DoctorAddCommand, Response<DoctorAddResponseDto>>
{
    private readonly IDoctorAddRepository _repo;
    private readonly IUnitOfWork _unitOfWork;



    public DoctorAddCommandHandler(
        IDoctorAddRepository repo,
        IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }


    // Step1: Check if Doctor already exists
    // Step2: if exists return success
    // Step3: if not, Save new Doctor
    // Step4: return success

    public async Task<Response<DoctorAddResponseDto>> Handle(DoctorAddCommand request, CancellationToken cancellationToken)
    {
        // Check if Doctor already exists
        Doctor Doctor = await _repo.Get(
            t => t.LastName == request.requestDto.LastName &&
            t.PhoneNumber == request.requestDto.PhoneNumber &&
            t.IsActive == true
        );

        // if exists return success
        if (Doctor is not null)
            return new DoctorAddResponseDto("Success");


        // Save new Doctor
        await _repo.Add(request.requestDto.New());


        // Commit
        var commitResult = await _unitOfWork.SaveChangesAsync();
        if (commitResult.IsFailure)
            return commitResult.Error;

        // Return Success
        return new DoctorAddResponseDto("Success");
    }



}



