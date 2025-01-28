using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.exceptions
{
    internal class ISBNAlreadyExistException : Exception
    {
        public ISBNAlreadyExistException() : base("This ISBN already exist in database. Try Again!")
        {

        }

        public ISBNAlreadyExistException(string message) : base(message)
        {

        }
    }
}
