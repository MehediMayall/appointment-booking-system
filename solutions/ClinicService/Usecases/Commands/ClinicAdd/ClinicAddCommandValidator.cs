namespace ClinicService;

public sealed class ClinicAddCommandValidator : AbstractValidator<ClinicAddCommand> {
    public ClinicAddCommandValidator() {
        
        RuleFor(x => x.requestDto.Name).NotEmpty().MinimumLength(2).MaximumLength(250);
        RuleFor(x => x.requestDto.Address).NotEmpty().MinimumLength(2).MaximumLength(250);
        
    }

}
