using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BCrypt.Net;
using LibraryProject.controllers;
using LibraryProject.exceptions;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace LibraryProject.abstracts
{
    internal abstract class UserAbstract
    {
        private string userName { get; set; }
        private string password { get; set; }
        private int userId { get; set; }

        public UserAbstract(string userName, string password) { 
        
            
            string pathToTxt = "../../../libraries/userIdCounter.txt";
            int nextIdFromFile;
            try
            {
                using (StreamReader nextId = new StreamReader(pathToTxt)) {
                    nextIdFromFile = int.Parse(nextId.ReadLine());
                }
                
                this.userId = nextIdFromFile;

                nextIdFromFile++;
                using (StreamWriter saveNextId = new StreamWriter(pathToTxt, false)) {
                    saveNextId.WriteLine(nextIdFromFile);
                }

            }
            catch (FileNotFoundException e) { 
                Console.WriteLine("UserAbstract fail: file userIdCounter.txt not found");
            }


            this.userName = validateUsername(userName);

            this.password = BCrypt.Net.BCrypt.HashPassword(password, 10);
        }

        protected UserAbstract(string userName, string password, int id) {
            this.userName = userName;
            this.password = password;
            this.userId=id;
        }

        public override string ToString()
        {
            return $"{this.userName},{this.password},{this.userId}";
        }

        public string getUserName() {
            return userName;
        }

        public string getPassword()
        {
            return password;
        }

        public int getId()
        {
            return userId;
        }

        private string validateUsername(string userName) {

            bool flag = true;
            string validUserName = userName;
            do
            {
                try
                {
                    if (!UserController.CheckUsername(validUserName))
                    {
                        flag = false;
                    }
                }
                catch (UserAlreadyExistException e)
                {
                    Console.Write($"{e.Message} Please input not used username \n");
                    validUserName = Console.ReadLine();
                }
            } while (flag);

            return validUserName;
        }
    }
}
