using FluentValidation;

namespace Expensi.Api.Expenses.Dtos;

public record CreateExpenseDto(
    string Title,
    decimal Amount,
    string Currency,
    DateTime Date,
    string Category,
    string Remitter);

public class CreateExpenseRequestValidator : AbstractValidator<CreateExpenseDto>
{
    public CreateExpenseRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200);

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be positive.");

        RuleFor(x => x.Currency)
            .NotEmpty();

        RuleFor(x => x.Date)
            .NotEmpty();
    }
}
