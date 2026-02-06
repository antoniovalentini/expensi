using FluentValidation;

namespace Expensi.Api.Remitters.Dtos;

public record CreateRemitterDto(string Name);

public class CreateRemitterRequestValidator : AbstractValidator<CreateRemitterDto>
{
    public CreateRemitterRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200);
    }
}
