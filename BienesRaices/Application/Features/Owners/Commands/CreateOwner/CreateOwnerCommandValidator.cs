using FluentValidation;

namespace Application.Features.Owners.Commands.CreateOwner
{
    public class CreateOwnerCommandValidator : AbstractValidator<CreateOwnerCommand>
    {
        public CreateOwnerCommandValidator()
        {
            RuleFor(x => x.Owner).NotNull();
            RuleFor(x => x.Owner.Name).NotEmpty().MaximumLength(250);
            RuleFor(x => x.Owner.Address).MaximumLength(500);
            RuleFor(x => x.Owner.Birthday).LessThanOrEqualTo(DateTime.Today).When(x => x.Owner.Birthday.HasValue);
            RuleFor(x => x.Owner.Photo)
                 .NotNull()
                 .WithMessage("Debe adjuntar una imagen del propietario.");
        }
    }
}
