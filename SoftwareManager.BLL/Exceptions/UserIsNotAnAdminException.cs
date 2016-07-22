using System;

namespace SoftwareManager.BLL.Exceptions
{
    public class UserIsNotAnAdminException : Exception
    {
        public UserIsNotAnAdminException()
        {

        }

        public UserIsNotAnAdminException(string message) : base(message)
        {

        }
    }
}