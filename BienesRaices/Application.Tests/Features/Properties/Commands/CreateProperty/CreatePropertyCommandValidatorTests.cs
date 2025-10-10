using Application.Features.Properties.Commands.CreateProperty;
using Application.DTOs.Properties;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System;

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
        public void Validator_WithValidModel_Passes()
        {
            var model = new CreatePropertyCommand { Property = new CreatePropertyDto { Title = "Nice", Price = 100, CodeInternal = "ABC", IdOwner = Guid.NewGuid() } };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Validator_MissingTitle_Fails()
        {
            var model = new CreatePropertyCommand { Property = new CreatePropertyDto { Title = "", Price = 100, CodeInternal = "ABC", IdOwner = Guid.NewGuid() } };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Property.Title);
        }

        [Test]
        public void Validator_NegativePrice_Fails()
        {
            var model = new CreatePropertyCommand { Property = new CreatePropertyDto { Title = "T", Price = 0, CodeInternal = "ABC", IdOwner = Guid.NewGuid() } };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Property.Price);
        }
    }
}
// ...existing code...
