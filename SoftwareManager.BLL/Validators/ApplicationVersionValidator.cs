using FluentValidation;
using SoftwareManager.BLL.Contracts.Models;

namespace SoftwareManager.BLL.Validators
{
    public class ApplicationVersionValidator : AbstractValidator<ApplicationVersion>
    {
        public ApplicationVersionValidator()
        {
            RuleFor(item => item.VersionNumber).NotEmpty().WithMessage("Please specify a version number");
            RuleFor(item => item.ReleaseDate).NotEmpty().WithMessage("Please specify a release date");
        }
    }
}