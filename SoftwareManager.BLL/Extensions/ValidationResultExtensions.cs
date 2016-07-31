using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using SoftwareManager.BLL.Models;

namespace SoftwareManager.BLL.Extensions
{
    public static class ValidationResultExtensions
    {
        public static IList<ModelError> ExtractModelErrors(this ValidationResult result)
        {
            return result.Errors.Select(item => new ModelError()
            {
                ErrorMessage = item.ErrorMessage,
                Property = item.PropertyName
            }).ToList();
        }
    }
}
