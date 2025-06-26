namespace PatientService;

public sealed class PatientAddCommandValidator : AbstractValidator<PatientAddCommand> {
    public PatientAddCommandValidator() {
        
        // RuleFor(x => x.requestDto.Name).NotEmpty().MinimumLength(2).MaximumLength(250);
        // RuleFor(x => x.requestDto.Address).NotEmpty().MinimumLength(2).MaximumLength(250);
        
    }

}
