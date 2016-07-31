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
    public class An_application_manager
    {

        private IValidator<ApplicationManager> _validator;
        public An_application_manager()
        {
            _validator = new ApplicationManagerValidator();
        }

        private ApplicationManager ArrangeManager(string name = null, string login = null)
        {
            ApplicationManager manager = new ApplicationManager();
            manager.Id = 1;
            manager.IsActive = true;
            manager.IsAdmin = true;
            manager.LoginName = login;
            manager.Name = name;
            return manager;
        }

        [Theory]
        [InlineData("Validate", "Login")]
        public void Should_be_valid_with_name_and_login(string name, string login)
        {
            var manager = ArrangeManager(name, login);
            var validationResult = _validator.Validate(manager);
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("Validate", "")]
        [InlineData("Validate", null)]
        public void Should_be_invalid_without_login(string name, string login)
        {
            var manager = ArrangeManager(name: name, login: login);
            var validationResult = _validator.Validate(manager);

            validationResult.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("", "Login")]
        [InlineData(null,"Login")]
        public void Should_be_invalid_without_name(string name,string login)
        {
            var manager = ArrangeManager(name: name, login: login);
            var validationResult = _validator.Validate(manager);

            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_be_invalid_without_name_and_login()
        {
            var manager = ArrangeManager();
            var validationResult = _validator.Validate(manager);
            validationResult.IsValid.Should().BeFalse();
        }
    }
}
