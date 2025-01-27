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
        public void Register(string userName, string password)
        {
            RegisterUser(userName, password);
        }



        public static async Task<List<User>> FetchUsers()
        {

            try
            {
                string json = File.ReadAllText(pathToUsersDB); // Wczytanie zawartości pliku

                var users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(json);

                return users;

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("UserController fail: UserDB.json file not found");
            }

            return new List<User>();


        }

        public static async Task<User> getUserAsync(string username)
        {
            List<User> users = await FetchUsers();

            User foundUser = null;

            foreach (User x in users)
            {
                if (x.getUserName().ToUpper() == username.ToUpper())
                {
                    foundUser = x; break;
                }
            }
            return foundUser;
        }


        public static async Task SaveUpdatedUser(User updatedUser)
        {
            var users = await UserController.FetchUsers(); // Pobranie wszystkich użytkowników z pliku

            // Znalezienie indeksu użytkownika, którego dane chcemy zaktualizować
            var userIndex = users.FindIndex(u => u.userId == updatedUser.userId);

            if (userIndex != -1)
            {
                // Aktualizacja danych użytkownika
                users[userIndex] = updatedUser;
            }
            else
            {
                Console.WriteLine("Nie znaleziono użytkownika w bazie danych.");
                return;
            }

            // Serializacja całej listy użytkowników z uwzględnieniem zmian
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });

            // Zapisanie całej listy użytkowników do pliku
            await File.WriteAllTextAsync("../../../libraries/UserDB.json", json);
        }



    }




}

