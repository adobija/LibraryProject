using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.classes;
using LibraryProject.controllers;
using LibraryProject.exceptions;

namespace LibraryProject.libraries
{
    internal class UsernameValidator
    {

        private static bool CheckUsername(List<User> listOfExistingUsers, string userName)
        {

            
            bool userNameAlredyExist = false;
            foreach (User user in listOfExistingUsers)
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

        public static string validateUsername(List<User> listOfExistingUsers, string userName)
        {

            bool flag = true;
            string validUserName = userName;
            do
            {
                try
                {
                    if (!CheckUsername(listOfExistingUsers, validUserName))
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
