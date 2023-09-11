using FluentValidation;

namespace Trickle.Directory.Application.Validators;

internal class EntityIdValidator : AbstractValidator<long?>
{
    public EntityIdValidator()
    {
        RuleFor(id => id)
            .GreaterThan(0)
            .When(id => id is not null);
    }
}
