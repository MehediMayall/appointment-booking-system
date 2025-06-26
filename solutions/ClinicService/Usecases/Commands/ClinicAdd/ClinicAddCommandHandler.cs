namespace ClinicService;

public record ClinicAddCommand(ClinicAddRequestDto requestDto) : IRequest<Response<ClinicAddResponseDto>>{}
public sealed class ClinicAddCommandHandler : IRequestHandler<ClinicAddCommand, Response<ClinicAddResponseDto>>
{
    private readonly IClinicAddRepository _repo;
    private readonly IUnitOfWork _unitOfWork;



    public ClinicAddCommandHandler(
        IClinicAddRepository repo,
        IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }


    // Step1: Check if Clinic already exists
    // Step2: if exists return success
    // Step3: if not, Save new Clinic
    // Step4: return success

    public async Task<Response<ClinicAddResponseDto>> Handle(ClinicAddCommand request, CancellationToken cancellationToken)
    {
        // Check if Clinic already exists
        Clinic clinic = await _repo.Get(
            t => t.Name == request.requestDto.Name &&
            t.Address == request.requestDto.Address &&
            t.IsActive == true
        );

        // if exists return success
        if (clinic is not null)
            return new ClinicAddResponseDto("Success");


        // Save new Clinic
        await _repo.Add(request.requestDto.New());


        // Commit
        var commitResult = await _unitOfWork.SaveChangesAsync();
        if (commitResult.IsFailure)
            return commitResult.Error;

        // Return Success
        return new ClinicAddResponseDto("Success");
    }



}



