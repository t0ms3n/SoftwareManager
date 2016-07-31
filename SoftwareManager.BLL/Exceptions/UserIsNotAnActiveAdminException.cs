using System;

namespace SoftwareManager.BLL.Exceptions
{
    public class UserIsNotAnActiveAdminException : Exception
    {
        public UserIsNotAnActiveAdminException()
        {

        }

        public UserIsNotAnActiveAdminException(string message) : base(message)
        {

        }
    }
}