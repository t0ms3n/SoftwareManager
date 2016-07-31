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
    public class When_updating_an_application : ContextSpecification
    {
        private Mock<ISoftwareManagerUoW> _softwareManagerUoW;
        private Mock<IIdentityService> _identityService;
        private Mock<IValidator<Application>> _applicationValidator;

        private IApplicationService _applicationService;
        private DataModels.Application _application = new DataModels.Application()
        {
            Id = 1,
             ApplicationIdentifier = Guid.Parse("E39B1D06-5736-4397-9D59-E81F3D8425C7"),
              Name = "My Application"
        };

        private Application _updateApplication = new Application()
        {
            Name = "My Application - Updated"
        };
        private Application _updatedApplication;

        //Arrange
        public override void EstablishContext()
        {
            var mockFactory = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };
            _softwareManagerUoW = mockFactory.Create<ISoftwareManagerUoW>();
            _applicationValidator = mockFactory.Create<IValidator<Application>>();

            _applicationValidator.Setup(f => f.ValidateAsync(It.IsAny<ValidationContext<Application>>(), CancellationToken.None)).Returns( () => Task.FromResult(new ValidationResult() {  }) );

            _identityService = mockFactory.Create<IIdentityService>();
            _identityService.SetupProperty(f => f.CurrentUser.Id, 1);

            _softwareManagerUoW.Setup(f => f.ApplicationRepository.GetAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(_application));

            _applicationService = new ApplicationService(_softwareManagerUoW.Object, _identityService.Object, _applicationValidator.Object);
        }

        //Act
        public override async Task Because()
        {
            _updatedApplication = await _applicationService.UpdateApplication(_application.Id, _updateApplication);
        }

        //Assert
        [Fact]
        public void the_update_application_should_be_validated()
        {
            // Should validate update manager
            _applicationValidator.Verify(f => f.ValidateAsync(It.Is<ValidationContext<Application>>(context => context.InstanceToValidate == _updateApplication ), CancellationToken.None), Times.AtLeastOnce);
        }

        //Assert
        [Fact]
        public void the_current_application_should_be_retrieved()
        {
            _softwareManagerUoW.Verify(f => f.ApplicationRepository.GetAsync(_application.Id), Times.Once);
        }

        //Assert
        [Fact]
        public void the_application_should_be_update_in_the_repository()
        {
            _softwareManagerUoW.Verify(f => f.ApplicationRepository.Update(It.IsAny<DataModels.Application>()), Times.Once);
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
            _updateApplication.Should().NotBeNull();
            _updateApplication.Id.ShouldBeEquivalentTo(_application.Id);
            _updateApplication.Name.ShouldBeEquivalentTo(_updatedApplication.Name);
            _updateApplication.Identifier.ShouldBeEquivalentTo(_updatedApplication.Identifier);
        }
    }
}
