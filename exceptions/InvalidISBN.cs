using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.exceptions
{
    internal class InvalidISBN : Exception
    {
        public InvalidISBN() : base("Provided invalid ISBN number") { 
            
        }

        public InvalidISBN(string message) : base(message)
        {

        }
    }
}
