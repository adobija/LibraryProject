using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.classes;
using LibraryProject.exceptions;
using LibraryProject.interfaces.implementations;
using LibraryProject.libraries;

namespace LibraryProject.controllers
{
    internal class UserController : UserRegisterImpl
    {

        public void Register(string userName, string password) {
            RegisterUser(userName, password);
        }

        public static List<User> FetchDbOfUsers()
        {
            List<User> users = RefactorUserDb.FetchUsers();
            return users;
        }

        public static bool CheckUsername(string userName)
        {

            List<User> listOfUsers = UserController.FetchDbOfUsers();
            bool userNameAlredyExist = false;
            foreach (User user in listOfUsers)
            {
                if (user.getUserName().ToUpper().Equals(userName.ToUpper()))
                {
                    userNameAlredyExist = true;
                    break;
                }
            }
            if (userNameAlredyExist)
            {
                throw new UserAlreadyExistException();
            }
            return userNameAlredyExist;
        }
    }
}
