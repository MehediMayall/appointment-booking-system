namespace AppointmentService;

public sealed class AppointmentConfirmCommandValidator : AbstractValidator<AppointmentConfirmCommand> {
    public AppointmentConfirmCommandValidator() {
        
        RuleFor(x => x.requestDto.AppointmentId).Must(ValidationMethods.BeAValidGuid).WithMessage("Please enter a valid appointment id.");

        
    }

}
