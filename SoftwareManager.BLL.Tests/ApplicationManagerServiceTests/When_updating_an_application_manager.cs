using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using SoftwareManager.BLL.Contracts.Models;
using SoftwareManager.BLL.Contracts.Services;
using SoftwareManager.BLL.Services;
using SoftwareManager.DAL.Contracts;
using DataModels = SoftwareManager.DAL.Contracts.Models;
using Xunit;

// ReSharper disable InconsistentNaming

    
namespace SoftwareManager.BLL.Tests.ApplicationManagerServiceTests
{
    public class When_updating_an_application_manager : ContextSpecification
    {
        private Mock<ISoftwareManagerUoW> _softwareManagerUoW;
        private Mock<IIdentityService> _identityService;
        private Mock<IValidator<ApplicationManager>> _applicationManagerValidator;

        private IApplicationManagerService _applicationManagerService;
        private DataModels.ApplicationManager _manager = new DataModels.ApplicationManager()
        {
            Id = 1,
            LoginName = "Current",
            Name = "Current Name"
        };

        private ApplicationManager _updateManager = new ApplicationManager()
        {
            LoginName = "Updated",
            Name = "Update Name"
        };
        private ApplicationManager _updatedManager;

        //Arrange
        public override void EstablishContext()
        {
            var mockFactory = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };
            _softwareManagerUoW = mockFactory.Create<ISoftwareManagerUoW>();
            _applicationManagerValidator = mockFactory.Create<IValidator<ApplicationManager>>();

            _applicationManagerValidator.Setup(f => f.ValidateAsync(It.IsAny<ValidationContext<ApplicationManager>>(), CancellationToken.None)).Returns( () => Task.FromResult(new ValidationResult() {  }) );

            _identityService = mockFactory.Create<IIdentityService>();
            _identityService.SetupProperty(f => f.CurrentUser.Id, 1);

            _softwareManagerUoW.Setup(f => f.ApplicationManagerRepository.GetAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(_manager));

            _applicationManagerService = new ApplicationManagerService(_softwareManagerUoW.Object, _identityService.Object, _applicationManagerValidator.Object);
        }

        //Act
        public override async Task Because()
        {
            _updatedManager = await _applicationManagerService.UpdateApplicationManager(_manager.Id, _updateManager);
        }

        //Assert
        [Fact]
        public void the_update_manager_should_be_validated()
        {
            // Should validate update manager
            _applicationManagerValidator.Verify(f => f.ValidateAsync(It.Is<ValidationContext<ApplicationManager>>(context => context.InstanceToValidate == _updateManager ), CancellationToken.None), Times.AtLeastOnce);
        }

        //Assert
        [Fact]
        public void the_user_must_be_an_admin()
        {
            _identityService.Verify( f => f.CheckAdminRole(), Times.AtLeastOnce);
        }

        //Assert
        [Fact]
        public void the_current_manager_should_be_retrieved()
        {
            _softwareManagerUoW.Verify(f => f.ApplicationManagerRepository.GetAsync(_manager.Id), Times.Once);
        }

        //Assert
        [Fact]
        public void the_manager_should_be_update_in_the_repository()
        {
            _softwareManagerUoW.Verify(f => f.ApplicationManagerRepository.Update(It.IsAny<DataModels.ApplicationManager>()), Times.Once);
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
            _updateManager.Should().NotBeNull();
            _updateManager.Id.ShouldBeEquivalentTo(_manager.Id);
            _updateManager.Name.ShouldBeEquivalentTo(_updatedManager.Name);
            _updateManager.LoginName.ShouldBeEquivalentTo(_updatedManager.LoginName);
        }
    }
}
