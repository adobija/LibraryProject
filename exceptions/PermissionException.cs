using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.exceptions
{
    internal class PermissionException : Exception
    {
        public PermissionException() : base("You don't have permision to do that action!")
        {

        }

        public PermissionException(string message) : base(message)
        {

        }
    }
}
