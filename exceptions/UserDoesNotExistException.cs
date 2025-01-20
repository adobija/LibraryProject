using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.exceptions
{
    internal class UserDoesNotExistException : Exception
    {

        public UserDoesNotExistException() : base("User with that username does not exist!")
        {

        }

        public UserDoesNotExistException(string message) : base(message)
        {

        }
    }
}
