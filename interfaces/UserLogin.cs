using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.classes;

namespace LibraryProject.interfaces
{
    internal interface UserLogin
    {
        static abstract Task<User> Login();
    }
}
