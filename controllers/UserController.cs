using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.classes;
using LibraryProject.exceptions;
using LibraryProject.interfaces.implementations;
using System.Text.Json;

namespace LibraryProject.controllers
{
    internal class UserController : UserRegisterImpl
    {
        private static string pathToUsersDB = "../../../libraries/UserDB.json";
        public void Register(string userName, string password) {
            RegisterUser(userName, password);
        }

        public static bool CheckUsername(string userName)
        {

            List<User> listOfUsers = new List<User>(); //UserController.FetchDbOfUsers();
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

        public async Task<List<User>> FetchUsers()
        {
            
            try
            {
                string json = await File.ReadAllTextAsync(pathToUsersDB); // Wczytanie zawartości pliku
                
                var users = JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true});

                foreach (User user in users) {
                    Console.WriteLine(user.getUserName());
                }

                return users;

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("UserController fail: UserDB.json file not found");
            }

            return new List<User>();


        }
    }
}
