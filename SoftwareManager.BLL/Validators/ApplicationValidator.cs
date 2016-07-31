using FluentValidation;
using SoftwareManager.BLL.Contracts.Models;

namespace SoftwareManager.BLL.Validators
{
    public class ApplicationValidator : AbstractValidator<Application>
    {
        public ApplicationValidator()
        {
            RuleFor(app => app.Name).NotEmpty().WithMessage("Please specify a name");
            RuleFor(app => app.Identifier).NotEmpty().WithMessage("Please specify an identifier");
        }
    }
}