using FluentValidation;

namespace Application.Features.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandValidator()
        {
            RuleFor(x => x.Property).NotNull();
            RuleFor(x => x.Property.Title).NotEmpty().MaximumLength(250);
            RuleFor(x => x.Property.Description).MaximumLength(1000);
            RuleFor(x => x.Property.Price).GreaterThan(0);
            RuleFor(x => x.Property.CodeInternal).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Property.IdOwner).NotEmpty();
        }
    }
}
