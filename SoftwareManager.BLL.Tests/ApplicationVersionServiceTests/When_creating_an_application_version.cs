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

    
namespace SoftwareManager.BLL.Tests.ApplicationVersionServiceTests
{
    public class When_creating_an_application_version : ContextSpecification
    {
        private Mock<ISoftwareManagerUoW> _softwareManagerUoW;
        private Mock<IIdentityService> _identityService;
        private Mock<IValidator<ApplicationVersion>> _applicationVersionValidator;

        private IApplicationVersionService _applicationVersionService;
        private ApplicationVersion _version = new ApplicationVersion()
        {
       
        };

        //Arrange
        public override void EstablishContext()
        {
            var mockFactory = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };
            _softwareManagerUoW = mockFactory.Create<ISoftwareManagerUoW>();
            _applicationVersionValidator = mockFactory.Create<IValidator<ApplicationVersion>>();

            _applicationVersionValidator.Setup(f => f.ValidateAsync(It.IsAny<ValidationContext<ApplicationVersion>>(), CancellationToken.None)).Returns( () => Task.FromResult(new ValidationResult() {  }) );

            _identityService = mockFactory.Create<IIdentityService>();
            _identityService.SetupProperty(f => f.CurrentUser.Id, 1);
            
            _applicationVersionService = new ApplicationVersionService(_softwareManagerUoW.Object, _identityService.Object, _applicationVersionValidator.Object);
        }

        //Act
        public override async Task Because()
        {
            await _applicationVersionService.CreateApplicationVersion(_version);
        }

        //Assert
        [Fact]
        public void the_version_should_be_validated()
        {
            _applicationVersionValidator.Verify( f => f.ValidateAsync(It.IsAny<ValidationContext<ApplicationVersion>>(), CancellationToken.None), Times.AtLeastOnce);
        }

        //Assert
        [Fact]
        public void the_version_should_be_added_to_the_repository()
        {
            _softwareManagerUoW.Verify(f => f.ApplicationVersionRepository.Add(It.IsAny<DataModels.ApplicationVersion>()), Times.Once);
        }

        //Assert
        [Fact]
        public void the_changes_should_be_saved()
        {
            _softwareManagerUoW.Verify(f => f.SaveAsync(), Times.Once);
        }
    }
}
