using FluentValidation;

namespace Application.Features.Properties.Commands.UpdatePropertyPrice
{
    public class UpdatePropertyPriceCommandValidator : AbstractValidator<UpdatePropertyPriceCommand>
    {
        public UpdatePropertyPriceCommandValidator()
        {
            RuleFor(x => x.IdProperty).NotEmpty();
            RuleFor(x => x.NewPrice).GreaterThan(0);
        }
    }
}
