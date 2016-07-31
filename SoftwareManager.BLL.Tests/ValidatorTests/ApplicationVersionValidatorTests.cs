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
    public class An_application_version
    {

        private IValidator<ApplicationVersion> _validator;
        public An_application_version()
        {
            _validator = new ApplicationVersionValidator();
        }

        private ApplicationVersion ArrangeVersion(string versionNumber = null, DateTime? releaseDate = null)
        {
            ApplicationVersion item = new ApplicationVersion();
            item.Id = 1;
            item.IsActive = true;
            item.IsCurrent = true;
            item.VersionNumber = versionNumber;
            if (releaseDate.HasValue)
            {
                item.ReleaseDate = releaseDate.Value;
            }
            return item;
        }

        [Fact]
        public void Should_be_valid_with_version_number_and_release_date()
        {
            var version = ArrangeVersion("1.0", new DateTime(2016, 1, 1));
            var validationResult = _validator.Validate(version);
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_be_invalid_without_version_number(string versionNumber)
        {
            var version = ArrangeVersion(versionNumber, new DateTime(2016, 1, 1));
            var validationResult = _validator.Validate(version);
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_be_invalid_without_release_date()
        {
            var version = ArrangeVersion("1.0", null);
            var validationResult = _validator.Validate(version);

            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_be_invalid_without_version_number_and_release_date()
        {
            var version = ArrangeVersion();
            var validationResult = _validator.Validate(version);
            validationResult.IsValid.Should().BeFalse();
        }
    }
}
