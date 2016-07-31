using FluentValidation;
using SoftwareManager.BLL.Contracts.Models;

namespace SoftwareManager.BLL.Validators
{

    public class ApplicationManagerValidator : AbstractValidator<ApplicationManager>
    {
        public ApplicationManagerValidator()
        {
            RuleFor(app => app.Name).NotEmpty().WithMessage("Please specify a name");
            RuleFor(app => app.LoginName).NotEmpty().WithMessage("Please specify a login name");
        }
    }

}