using System.Threading.Tasks;
using Moq;
using SoftwareManager.BLL.Contracts.Services;
using SoftwareManager.BLL.Services;
using SoftwareManager.DAL.Contracts;
using Xunit;
using DataModels = SoftwareManager.DAL.Contracts.Models;

// ReSharper disable InconsistentNaming
namespace SoftwareManager.BLL.Tests.ApplicationServiceTests
{
    public class When_deleting_an_application : ContextSpecification
    {
        private Mock<ISoftwareManagerUoW> _softwareManagerUoW;
        private Mock<IIdentityService> _identityService;

        private IApplicationService _applicationService;
        private DataModels.Application _version = new DataModels.Application()
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

            _softwareManagerUoW.Setup(f => f.ApplicationRepository.GetAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(_version));

            _applicationService = new ApplicationService(_softwareManagerUoW.Object, _identityService.Object, null);
        }

        //Act
        public override async Task Because()
        {
            await _applicationService.DeleteApplication(_version.Id);
        }
        
        //Assert
        [Fact]
        public void the_current_application_should_be_retrieved()
        {
            _softwareManagerUoW.Verify(f => f.ApplicationRepository.GetAsync(_version.Id), Times.Once);
        }

        //Assert
        [Fact]
        public void the_application_should_be_removed_from_the_repository()
        {
            _softwareManagerUoW.Verify(f => f.ApplicationRepository.Remove(It.IsAny<DataModels.Application>()), Times.Once);
        }

        //Assert
        [Fact]
        public void the_changes_should_be_saved()
        {
            _softwareManagerUoW.Verify(f => f.SaveAsync(), Times.Once);
        }
    }
}
