using System.Threading.Tasks;
using Moq;
using SoftwareManager.BLL.Contracts.Services;
using SoftwareManager.BLL.Services;
using SoftwareManager.DAL.Contracts;
using Xunit;
using DataModels = SoftwareManager.DAL.Contracts.Models;

// ReSharper disable InconsistentNaming
namespace SoftwareManager.BLL.Tests.ApplicationVersionServiceTests
{
    public class When_deleting_an_application_version : ContextSpecification
    {
        private Mock<ISoftwareManagerUoW> _softwareManagerUoW;
        private Mock<IIdentityService> _identityService;

        private IApplicationVersionService _applicationVersionService;
        private DataModels.ApplicationVersion _version = new DataModels.ApplicationVersion()
        {
            Id = 1
        };
        
        //Arrange
        public override void EstablishContext()
        {
            var mockFactory = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };
            _softwareManagerUoW = mockFactory.Create<ISoftwareManagerUoW>();

            _identityService = mockFactory.Create<IIdentityService>();
            _identityService.SetupProperty(f => f.CurrentUser.Id, 1);  

            _softwareManagerUoW.Setup(f => f.ApplicationVersionRepository.GetAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(_version));

            _applicationVersionService = new ApplicationVersionService(_softwareManagerUoW.Object, _identityService.Object, null);
        }

        //Act
        public override async Task Because()
        {
            await _applicationVersionService.DeleteApplicationVersion(_version.Id);
        }
        
        //Assert
        [Fact]
        public void the_current_vesion_should_be_retrieved()
        {
            _softwareManagerUoW.Verify(f => f.ApplicationVersionRepository.GetAsync(_version.Id), Times.Once);
        }

        //Assert
        [Fact]
        public void the_version_should_be_removed_from_the_repository()
        {
            _softwareManagerUoW.Verify(f => f.ApplicationVersionRepository.Remove(It.IsAny<DataModels.ApplicationVersion>()), Times.Once);
        }

        //Assert
        [Fact]
        public void the_changes_should_be_saved()
        {
            _softwareManagerUoW.Verify(f => f.SaveAsync(), Times.Once);
        }
    }
}
