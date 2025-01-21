﻿using System;
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

        

        public static async Task<List<User>> FetchUsers()
        {
            
            try
            {
                string json = File.ReadAllText(pathToUsersDB); // Wczytanie zawartości pliku
                
                var users = JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true});

                return users;

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("UserController fail: UserDB.json file not found");
            }

            return new List<User>();


        }

        public static async Task<User> getUserAsync(string username) { 
            List<User> users = await FetchUsers();

            User foundUser = null;

            foreach (User x in users)
            {
                if (x.getUserName().ToUpper() == username.ToUpper()) { 
                foundUser = x; break;
                }
            }
            return foundUser;
        }
    }
}
