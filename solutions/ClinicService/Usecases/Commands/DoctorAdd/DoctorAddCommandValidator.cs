namespace ClinicService;

public sealed class DoctorAddCommandValidator : AbstractValidator<DoctorAddCommand> {
    public DoctorAddCommandValidator() {
        
        RuleFor(x => x.requestDto.FirstName).NotEmpty().MinimumLength(2).MaximumLength(250);
        RuleFor(x => x.requestDto.LastName).NotEmpty().MinimumLength(2).MaximumLength(250);
        RuleFor(x => x.requestDto.Specialization).NotEmpty().MinimumLength(2).MaximumLength(250);
        RuleFor(x => x.requestDto.PhoneNumber).NotEmpty().MinimumLength(2).MaximumLength(15);
        
    }

}
