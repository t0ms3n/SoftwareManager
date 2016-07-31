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
using Xunit.Sdk;

// ReSharper disable InconsistentNaming

    
namespace SoftwareManager.BLL.Tests.ApplicationManagerServiceTests
{
    public class When_creating_an_application_manager : ContextSpecification
    {
        private Mock<ISoftwareManagerUoW> _softwareManagerUoW;
        private Mock<IIdentityService> _identityService;
        private Mock<IValidator<ApplicationManager>> _applicationManagerValidator;

        private IApplicationManagerService _applicationManagerService;
        private ApplicationManager _manager = new ApplicationManager()
        {
            LoginName = "Test",
            Name = "abc"
        };

        //Arrange
        public override void EstablishContext()
        {
            var mockFactory = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };
            _softwareManagerUoW = mockFactory.Create<ISoftwareManagerUoW>();
            _applicationManagerValidator = mockFactory.Create<IValidator<ApplicationManager>>();

            _applicationManagerValidator.Setup(f => f.ValidateAsync(It.IsAny<ValidationContext<ApplicationManager>>(), CancellationToken.None)).Returns( () => Task.FromResult(new ValidationResult() {  }) );

            _identityService = mockFactory.Create<IIdentityService>();
            _identityService.SetupProperty(f => f.CurrentUser.Id, 1);
            
            _applicationManagerService = new ApplicationManagerService(_softwareManagerUoW.Object, _identityService.Object, _applicationManagerValidator.Object);
        }

        //Act
        public override async Task Because()
        {
            await _applicationManagerService.CreateApplicationManager(_manager);
        }

        //Assert
        [Fact]
        public void the_manager_should_be_validated()
        {
            _applicationManagerValidator.Verify( f => f.ValidateAsync(It.IsAny<ValidationContext<ApplicationManager>>(), CancellationToken.None), Times.AtLeastOnce);
        }

        //Assert
        [Fact]
        public void the_user_must_be_an_admin()
        {
            _identityService.Verify( f => f.CheckAdminRole(), Times.AtLeastOnce);
        }

        //Assert
        [Fact]
        public void the_manager_should_be_added_to_the_repository()
        {
            _softwareManagerUoW.Verify(f => f.ApplicationManagerRepository.Add(It.IsAny<DataModels.ApplicationManager>()), Times.Once);
        }

        //Assert
        [Fact]
        public void the_changes_should_be_saved()
        {
            _softwareManagerUoW.Verify(f => f.SaveAsync(), Times.Once);
        }
    }
}
