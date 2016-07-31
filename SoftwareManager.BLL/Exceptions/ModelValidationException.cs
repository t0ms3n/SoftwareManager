using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoftwareManager.BLL.Models;

namespace SoftwareManager.BLL.Exceptions
{
    public class ModelValidationException : Exception
    {
        public IList<ModelError> ModelErrors { get; }

        public ModelValidationException()
        {
            ModelErrors = new List<ModelError>();
        }

        public ModelValidationException(string message, IList<ModelError> modelErrors ) : base(message)
        {
            ModelErrors = modelErrors;
        }
    }
}
