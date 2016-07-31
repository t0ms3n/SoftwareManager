using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using SoftwareManager.BLL.Contracts.Services;
using SoftwareManager.BLL.Services;
using SoftwareManager.DAL.Contracts;
using Xunit;
using DataModels = SoftwareManager.DAL.Contracts.Models;
using SoftwareManager.BLL.Contracts.Models;

// ReSharper disable InconsistentNaming


namespace SoftwareManager.BLL.Tests.ApplicationVersionServiceTests
{
    public class When_updating_an_application_version : ContextSpecification
    {
        private Mock<ISoftwareManagerUoW> _softwareManagerUoW;
        private Mock<IIdentityService> _identityService;
        private Mock<IValidator<ApplicationVersion>> _applicationVersionValidator;

        private IApplicationVersionService _applicationVersionService;
        private DataModels.ApplicationVersion _version = new DataModels.ApplicationVersion()
        {
            Id = 1,
            ApplicationId = 1,
            IsActive = true,
            IsCurrent = false,
            VersionNumber = "1.0",
            ReleaseDate = new DateTime(2016,1,1)
        };

        private ApplicationVersion _updateVersion = new ApplicationVersion()
        {
            VersionNumber = "1.1",
            IsCurrent = true
        };
        private ApplicationVersion _updatedVersion;

        //Arrange
        public override void EstablishContext()
        {
            var mockFactory = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };
            _softwareManagerUoW = mockFactory.Create<ISoftwareManagerUoW>();
            _applicationVersionValidator = mockFactory.Create<IValidator<ApplicationVersion>>();

            _applicationVersionValidator.Setup(f => f.ValidateAsync(It.IsAny<ValidationContext<ApplicationVersion>>(), CancellationToken.None)).Returns( () => Task.FromResult(new ValidationResult() {  }) );

            _identityService = mockFactory.Create<IIdentityService>();
            _identityService.SetupProperty(f => f.CurrentUser.Id, 1);

            _softwareManagerUoW.Setup(f => f.ApplicationVersionRepository.GetAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(_version));

            _applicationVersionService = new ApplicationVersionService(_softwareManagerUoW.Object, _identityService.Object, _applicationVersionValidator.Object);
        }

        //Act
        public override async Task Because()
        {
            _updatedVersion = await _applicationVersionService.UpdateApplicationVersion(_version.Id, _updateVersion);
        }

        //Assert
        [Fact]
        public void the_update_version_should_be_validated()
        {
            // Should validate update manager
            _applicationVersionValidator.Verify(f => f.ValidateAsync(It.Is<ValidationContext<ApplicationVersion>>(context => context.InstanceToValidate == _updateVersion ), CancellationToken.None), Times.AtLeastOnce);
        }

        //Assert
        [Fact]
        public void the_current_version_should_be_retrieved()
        {
            _softwareManagerUoW.Verify(f => f.ApplicationVersionRepository.GetAsync(_version.Id), Times.Once);
        }

        //Assert
        [Fact]
        public void the_version_should_be_update_in_the_repository()
        {
            _softwareManagerUoW.Verify(f => f.ApplicationVersionRepository.Update(It.IsAny<DataModels.ApplicationVersion>()), Times.Once);
        }

        //Assert
        [Fact]
        public void the_changes_should_be_saved()
        {
            _softwareManagerUoW.Verify(f => f.SaveAsync(), Times.Once);
        }

        //Assert
        [Fact]
        public void the_update_should_have_the_new_values()
        {
            _updateVersion.Should().NotBeNull();
            _updateVersion.Id.ShouldBeEquivalentTo(_version.Id);
            _updateVersion.VersionNumber.ShouldBeEquivalentTo(_updatedVersion.VersionNumber);
            _updateVersion.IsActive.ShouldBeEquivalentTo(_updatedVersion.IsActive);
            _updateVersion.IsCurrent.ShouldBeEquivalentTo(_updatedVersion.IsCurrent);
        }
    }
}
