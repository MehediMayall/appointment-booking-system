namespace ClinicService;

public record DoctorSlotAddCommand(DoctorSlotAddRequestDto requestDto) : IRequest<Response<DoctorSlotAddResponseDto>>{}
public sealed class DoctorSlotAddCommandHandler : IRequestHandler<DoctorSlotAddCommand, Response<DoctorSlotAddResponseDto>>
{
    private readonly IDoctorSlotAddRepository _repo;
    private readonly IUnitOfWork _unitOfWork;



    public DoctorSlotAddCommandHandler(
        IDoctorSlotAddRepository repo,
        IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }


    // Step1: Create new DoctorSlot
    // Step2: Check if Doctor Slot already exists
    // Step3: if exists return success
    // Step4: Check if there are overlapping slots
    // Step5: if not, Save new DoctorSlot
    // Step6: return success

    public async Task<Response<DoctorSlotAddResponseDto>> Handle(DoctorSlotAddCommand request, CancellationToken cancellationToken)
    {
        // Create new DoctorSlot
        Slot newSlot = request.requestDto.New();

        // Check if DoctorSlot already exists
        Slot existingSlot = await _repo.Get(
            t => t.DoctorId == request.requestDto.DoctorId &&
            t.ClinicId == request.requestDto.ClinicId &&
            t.StartTime == newSlot.StartTime&&
            t.EndTime == newSlot.EndTime &&
            t.IsActive == true
        );


        // if exists return success
        if (existingSlot is not null)
            return new DoctorSlotAddResponseDto("Success");

        
        // Check if there are overlapping slots
        if (await _repo.HasOverlappingSlots(newSlot.ClinicId, newSlot.DoctorId, newSlot.StartTime, newSlot.EndTime))
            return DoctorSlotAddErrors.HasOverlappingSlots();


        // Save new DoctorSlot
        await _repo.Add(newSlot);


        // Commit
        var commitResult = await _unitOfWork.SaveChangesAsync();
        if (commitResult.IsFailure)
            return commitResult.Error;

        // Return Success
        return new DoctorSlotAddResponseDto("Success");
    }



}



