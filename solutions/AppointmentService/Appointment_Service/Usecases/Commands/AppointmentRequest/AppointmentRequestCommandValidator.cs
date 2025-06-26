namespace AppointmentService;

public sealed class AppointmentRequestCommandValidator : AbstractValidator<AppointmentRequestCommand> {
    public AppointmentRequestCommandValidator() {
        
        RuleFor(x => x.requestDto.DoctorId).Must(ValidationMethods.BeAValidGuid).WithMessage("Please enter a valid doctor id.");
        RuleFor(x => x.requestDto.PatientId).Must(ValidationMethods.BeAValidGuid).WithMessage("Please enter a valid patient id.");
        RuleFor(x => x.requestDto.SlotId).Must(ValidationMethods.BeAValidGuid).WithMessage("Please enter a valid slot id.");

        
    }

}
