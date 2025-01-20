using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.classes;
using LibraryProject.controllers;

namespace LibraryProject.interfaces.implementations
{
    internal class UserLoginImpl : UserLogin
    {
        public static async Task<User> Login()
        {
            //pobierz wszystkich uzytkownikow z bazy
            //przetestuj podany login
            //zapytaj o haslo
            //sprawdz czy haslo zgadza się z haslem podanym do loginu
            Console.WriteLine("Input your username");
            string username = Console.ReadLine().ToUpper();

            User foundUser = await UserController.getUserAsync(username);

            Console.WriteLine("Input your password");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Console.ReadLine(), 10);

            if (BCrypt.Net.BCrypt.Verify(hashedPassword, foundUser.getPassword()))
            {
                Console.WriteLine($"Welcome {foundUser.getUserName()}");
                return foundUser;
            }

            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("Wrong password! Try Again");

                hashedPassword = BCrypt.Net.BCrypt.HashPassword(Console.ReadLine(), 10);

                if (BCrypt.Net.BCrypt.Verify(hashedPassword, foundUser.getPassword()))
                {
                    Console.WriteLine($"Welcome {foundUser.getUserName()}");
                    return foundUser;
                }
                else {
                    continue;
                }

            }
            Console.WriteLine("Incorrect password... Exiting the application...");
            return null;


        }
    }
}
