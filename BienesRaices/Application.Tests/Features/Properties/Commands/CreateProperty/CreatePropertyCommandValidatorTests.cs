using Application.Features.Properties.Commands.CreateProperty;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.Tests.Features.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommandValidatorTests
    {
        private CreatePropertyCommandValidator _validator = null!;

        [SetUp]
        public void Setup()
        {
            _validator = new CreatePropertyCommandValidator();
        }

        [Test]
        public void Validator_Should_Have_Error_When_Title_Is_Empty()
        {
            var model = new CreatePropertyCommand { Property = new Application.DTOs.Properties.CreatePropertyDto { Title = "" } };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Property.Title);
        }

        [Test]
        public void Validator_Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var model = new CreatePropertyCommand { Property = new Application.DTOs.Properties.CreatePropertyDto { Title = "Nice", Price = 100, CodeInternal = "ABC", IdOwner = Guid.NewGuid() } };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
