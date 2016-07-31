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
    public class When_deleting_an_application_manager : ContextSpecification
    {
        private Mock<ISoftwareManagerUoW> _softwareManagerUoW;
        private Mock<IIdentityService> _identityService;

        private IApplicationManagerService _applicationManagerService;
        private DataModels.ApplicationManager _manager = new DataModels.ApplicationManager()
        {
            Id = 1,
            LoginName = "Current",
            Name = "Current Name"
        };
        
        //Arrange
        public override void EstablishContext()
        {
            var mockFactory = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };
            _softwareManagerUoW = mockFactory.Create<ISoftwareManagerUoW>();

            _identityService = mockFactory.Create<IIdentityService>();
            _identityService.SetupProperty(f => f.CurrentUser.Id, 1);  

            _softwareManagerUoW.Setup(f => f.ApplicationManagerRepository.GetAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(_manager));

            _applicationManagerService = new ApplicationManagerService(_softwareManagerUoW.Object, _identityService.Object, null);
        }

        //Act
        public override async Task Because()
        {
            await _applicationManagerService.DeleteApplicationManager(_manager.Id);
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
        public void the_manager_should_be_removed_from_the_repository()
        {
            _softwareManagerUoW.Verify(f => f.ApplicationManagerRepository.Remove(It.IsAny<DataModels.ApplicationManager>()), Times.Once);
        }

        //Assert
        [Fact]
        public void the_changes_should_be_saved()
        {
            _softwareManagerUoW.Verify(f => f.SaveAsync(), Times.Once);
        }
    }
}
