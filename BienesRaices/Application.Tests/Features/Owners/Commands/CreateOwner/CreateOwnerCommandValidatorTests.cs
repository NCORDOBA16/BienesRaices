using Application.Features.Owners.Commands.CreateOwner;
using Application.DTOs.Owners;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System;

namespace Application.Tests.Features.Owners.Commands.CreateOwner
{
    public class CreateOwnerCommandValidatorTests
    {
        private CreateOwnerCommandValidator _validator = null!;

        [SetUp]
        public void Setup()
        {
            _validator = new CreateOwnerCommandValidator();
        }

        [Test]
        public void Validator_WithValidModel_Passes()
        {
            var model = new CreateOwnerCommand { Owner = new CreateOwnerDto { Name = "Name", Address = "Addr", Birthday = DateTime.Today.AddYears(-20) } };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Validator_MissingName_Fails()
        {
            var model = new CreateOwnerCommand { Owner = new CreateOwnerDto { Name = "", Address = "Addr" } };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Owner.Name);
        }

        [Test]
        public void Validator_BirthdayInFuture_Fails()
        {
            var model = new CreateOwnerCommand { Owner = new CreateOwnerDto { Name = "N", Birthday = DateTime.Today.AddDays(1) } };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Owner.Birthday);
        }
    }
}
