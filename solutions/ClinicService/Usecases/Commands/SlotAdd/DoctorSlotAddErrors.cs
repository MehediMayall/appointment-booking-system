namespace ClinicService;


public sealed class DoctorSlotAddErrors : ExceptionBase<DoctorSlotAddRequestDto> {

    public static Error HasOverlappingSlots() =>
        new("Overlapping Slots ", $"This slot is overlapping with another slot. Please try again with a different slot.");
}

