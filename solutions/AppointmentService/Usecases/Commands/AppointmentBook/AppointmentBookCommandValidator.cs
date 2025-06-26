namespace AppointmentService;

public sealed class AppointmentBookCommandValidator : AbstractValidator<AppointmentBookCommand> {
    public AppointmentBookCommandValidator() {
        
        RuleFor(x => x.requestDto.Name).NotEmpty().MinimumLength(2).MaximumLength(250);
        RuleFor(x => x.requestDto.Address).NotEmpty().MinimumLength(2).MaximumLength(250);
        
    }

}
