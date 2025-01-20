using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.classes;
using LibraryProject.exceptions;
using LibraryProject.interfaces.implementations;
using LibraryProject.libraries;
using System.Text.Json;

namespace LibraryProject.controllers
{
    internal class UserController : UserRegisterImpl
    {
        private static string pathToUsersDB = "../../../libraries/UserDB.json";
        public void Register(string userName, string password) {
            RegisterUser(userName, password);
        }

        public static List<User> FetchDbOfUsers()
        {
            List<User> users = FetchUsers();
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

        public async static Task<List<User>> FetchUsers()
        {
            List<User> users = new List<User>();
            try
            {
                    string json = await File.ReadAllTextAsync(pathToUsersDB); // Wczytanie zawartości pliku

                    users =  JsonSerializer.Deserialize<List<User>>(json);

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("UserController fail: UserDB.json file not found");
            }

            return users;

        }
    }
}
