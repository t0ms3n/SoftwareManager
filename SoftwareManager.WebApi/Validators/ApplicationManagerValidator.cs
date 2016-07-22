using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using SoftwareManager.Entities;
using FluentValidation;

namespace SoftwareManager.WebApi.Validators
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