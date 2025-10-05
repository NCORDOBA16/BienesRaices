using FluentValidation;

namespace Application.Features.Properties.Commands.UploadPropertyImage
{
    public class UploadPropertyImageCommandValidator : AbstractValidator<UploadPropertyImageCommand>
    {
        public UploadPropertyImageCommandValidator()
        {
            RuleFor(x => x.PropertyId)
                .NotEmpty().WithMessage("{Property} must have a value")
                .NotNull().WithMessage("{Property} must have a value");

            RuleFor(x => x.PropertyImage)
                .NotNull().WithMessage("{Property} must have a content");
        }
    }
}
