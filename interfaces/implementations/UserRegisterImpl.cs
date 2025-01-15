using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.classes;

namespace LibraryProject.interfaces.implementations
{
    internal class UserRegisterImpl :UserRegister
    {
        public void RegisterUser(string userName, string password)
        {
            User user = new User(userName, password);

            string pathToUserDB = "../../../libraries/UserDB.txt";
            try
            {
                using (StreamWriter file = new StreamWriter(pathToUserDB, true)) {
                    file.WriteLine(user.ToString());
                }
                Success();
            }
            catch (FileNotFoundException e)
            { 
            Console.WriteLine("UserRegisterImpl fail: file UserDB.txt not found");
            }
        }

        public void Success()
        {
            Console.WriteLine("Successfully registered!");
        }

    }
}
