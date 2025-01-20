using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.exceptions
{
    internal class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() : base("Inserted 3 times incorrect password... Exiting the application...")
        {

        }

        public InvalidPasswordException(string message) : base(message)
        {

        }
    }
}
