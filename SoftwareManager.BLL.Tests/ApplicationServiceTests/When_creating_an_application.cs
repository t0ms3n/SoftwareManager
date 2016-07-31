using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using SoftwareManager.BLL.Contracts.Models;
using SoftwareManager.BLL.Contracts.Services;
using SoftwareManager.BLL.Services;
using SoftwareManager.DAL.Contracts;
using Xunit;

using DataModels = SoftwareManager.DAL.Contracts.Models;

// ReSharper disable InconsistentNaming

    
namespace SoftwareManager.BLL.Tests.ApplicationServiceTests
{
    public class When_creating_an_application : ContextSpecification
    {
        private Mock<ISoftwareManagerUoW> _softwareManagerUoW;
        private Mock<IIdentityService> _identityService;
        private Mock<IValidator<Application>> _applicationValidator;

        private IApplicationService _applicationService;
        private Application _application = new Application()
        {
       
        };

        //Arrange
        public override void EstablishContext()
        {
            var mockFactory = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };
            _softwareManagerUoW = mockFactory.Create<ISoftwareManagerUoW>();
            _applicationValidator = mockFactory.Create<IValidator<Application>>();

            _applicationValidator.Setup(f => f.ValidateAsync(It.IsAny<ValidationContext<Application>>(), CancellationToken.None)).Returns( () => Task.FromResult(new ValidationResult() {  }) );

            _identityService = mockFactory.Create<IIdentityService>();
            _identityService.SetupProperty(f => f.CurrentUser.Id, 1);
            
            _applicationService = new ApplicationService(_softwareManagerUoW.Object, _identityService.Object, _applicationValidator.Object);
        }

        //Act
        public override async Task Because()
        {
            await _applicationService.CreateApplication(_application);
        }

        //Assert
        [Fact]
        public void the_application_should_be_validated()
        {
            _applicationValidator.Verify( f => f.ValidateAsync(It.IsAny<ValidationContext<Application>>(), CancellationToken.None), Times.AtLeastOnce);
        }

        //Assert
        [Fact]
        public void the_version_should_be_added_to_the_repository()
        {
            _softwareManagerUoW.Verify(f => f.ApplicationRepository.Add(It.IsAny<DataModels.Application>()), Times.Once);
        }

        //Assert
        [Fact]
        public void the_changes_should_be_saved()
        {
            _softwareManagerUoW.Verify(f => f.SaveAsync(), Times.Once);
        }
    }
}
