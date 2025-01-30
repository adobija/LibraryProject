using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LibraryProject.classes;
using LibraryProject.controllers;
using LibraryProject.exceptions;
using LibraryProject.libraries;

namespace LibraryProject.interfaces.implementations
{
    internal class UserRegisterImpl : UserRegister
    {
        private string UserDB = @"../../../libraries/UserDB.json";

        public async void RegisterUser(string userName, string password)
        {
            
            List<User> listOfExistingUsers = await UserController.FetchUsers();

            //Console.WriteLine($"Number of users fetched: {listOfExistingUsers.Count}");
            
            string validUsername = UsernameValidator.validateUsername(listOfExistingUsers, userName);
            

            User user = new User(validUsername, password);

            listOfExistingUsers.Add(user);

            try
            {
                using (StreamWriter file = new StreamWriter(UserDB, false))
                {
                    
                    //listOfExistingUsers.Add(user);
                    //Console.WriteLine(user.getUserName());
                    string json = JsonSerializer.Serialize(listOfExistingUsers, new JsonSerializerOptions() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }); // Serializacja do JSON
                    //Console.WriteLine(json);
                    
                    await file.WriteAsync(json);
                }
                Success();
            }
            catch (FileNotFoundException e)
            { 
            Console.WriteLine("UserRegisterImpl fail: file UserDB.json not found");
            }
        }

        public static void StartRegister()
        {
            UserController userController = new UserController();

            Console.WriteLine("Create your username:");
            string userName = Console.ReadLine();

            bool flag = true;
            string password;
            do
            {
                Console.WriteLine("Create your password:");
                password = Console.ReadLine();

                Console.WriteLine("Repeat your password:");
                string secondPassword = Console.ReadLine();

                if (password.Equals(secondPassword))
                {
                    flag = false;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Passwords doesn't match. Try again");
                    continue;
                }
            } while (flag);

            userController.RegisterUser(userName, password);
        }

        public void Success()
        {
            Console.Clear();
            Console.WriteLine("Successfully registered!");
        }

    }
}
