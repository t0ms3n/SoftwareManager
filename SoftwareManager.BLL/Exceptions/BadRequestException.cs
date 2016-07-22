using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareManager.BLL.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException()
        {
            
        }

        public BadRequestException(string message) : base(message)
        {
            
        }
    }
}
