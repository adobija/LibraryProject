using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.exceptions
{
    internal class UserAlreadyExistException : Exception
    {

        public UserAlreadyExistException() : base("User with this username already exist!")
        {

        }

        public UserAlreadyExistException(string message) : base(message)
        {

        }
    }
}
