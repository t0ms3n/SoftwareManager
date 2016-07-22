using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SoftwareManager.Entities;
using FluentValidation;

namespace SoftwareManager.WebApi.Validators
{
    public class ApplicationValidator : AbstractValidator<Application>
    {
        public ApplicationValidator()
        {
            RuleFor(app => app.Name).NotEmpty().WithMessage("Please specify a name");
        }
    }

}