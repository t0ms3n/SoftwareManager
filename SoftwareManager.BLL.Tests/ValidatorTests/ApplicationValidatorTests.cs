using FluentAssertions;
using FluentValidation;
using SoftwareManager.BLL.Contracts.Models;
using SoftwareManager.BLL.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SoftwareManager.BLL.Tests.ValidatorTests
{
    public class An_application
    {

        private IValidator<Application> _validator;
        public An_application()
        {
            _validator = new ApplicationValidator();
        }

        private Application ArrangeApplication(string name = null, string identifier = null)
        {
            Application item = new Application();
            item.Id = 1;
            if (!string.IsNullOrEmpty(identifier))
            {
                item.Identifier = Guid.Parse(identifier);
            }

            item.Name = name;
            return item;
        }

        [Theory]
        [InlineData("Application", "{1E443466-D0D3-44AE-92AF-9F3D6EB64D04}")]
        public void Should_be_valid_with_name_and_identifier(string name, string identifier)
        {
            var application = ArrangeApplication(name, identifier);
            var validationResult = _validator.Validate(application);
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("Application")]
        public void Should_be_invalid_without_identifier(string name)
        {
            var application = ArrangeApplication(name: name);
            var validationResult = _validator.Validate(application);

            validationResult.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("","{1E443466-D0D3-44AE-92AF-9F3D6EB64D04}")]
        [InlineData(null, "{1E443466-D0D3-44AE-92AF-9F3D6EB64D04}")]
        public void Should_be_invalid_without_name(string name, string identifier)
        {
            var application = ArrangeApplication(name, identifier);
            var validationResult = _validator.Validate(application);

            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_be_invalid_without_name_and_identifer()
        {
            var application = ArrangeApplication();
            var validationResult = _validator.Validate(application);

            validationResult.IsValid.Should().BeFalse();
        }

    }
}
