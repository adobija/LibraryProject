using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.interfaces
{
    internal interface UserRegister
    {

        void RegisterUser(string userName, string password);

        void Success();

        static abstract void StartRegister();
    }
}
