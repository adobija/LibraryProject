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

            Console.WriteLine("Create your account!");
            Console.WriteLine("Insert username");
            string username = Console.ReadLine();
            Console.WriteLine("Insert password");
            string password = Console.ReadLine();

            userController.RegisterUser(username, password);
        }

        public void Success()
        {
            Console.WriteLine("Successfully registered!");
        }

    }
}
