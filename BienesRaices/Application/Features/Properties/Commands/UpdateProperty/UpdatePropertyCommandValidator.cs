using FluentValidation;

namespace Application.Features.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
    {
        public UpdatePropertyCommandValidator()
        {
            RuleFor(x => x.IdProperty).NotEmpty();
            When(x => x.Price.HasValue, () => {
                RuleFor(x => x.Price).GreaterThan(0);
            });
            When(x => x.Year.HasValue, () => {
                var current = DateTime.UtcNow.Year;
                RuleFor(x => x.Year).InclusiveBetween(1900, current + 1);
            });
        }
    }
}
