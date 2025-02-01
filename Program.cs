using LibraryProject.classes;
using LibraryProject.controllers;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using LibraryProject.interfaces.implementations;
using LibraryProject.exceptions;
using LibraryProject.libraries;


namespace LibraryProject
{
    internal class Program
    {
        
        static async Task Main(string[] args)
        {
            /*
             IMPORTANT 
            
             Username | Password

             Admin | P@$$w0rd

             Olek | P@$$w0rd

             Pawel | test123
            */

            Console.WriteLine("Welcome to library system!\n");
            User loggedUser = null;
            bool flag = true;
            do
            {

                Console.WriteLine("1: Login");
                Console.WriteLine("2: Register");
                string response = Console.ReadLine().ToUpper();
                Console.Clear();

                switch (response)
                {
                    case "1":
                    case "LOGIN":
                    case "L":
                        try
                        {
                            loggedUser = await UserLoginImpl.Login();
                        }
                        catch (InvalidPasswordException e)
                        {
                            Console.WriteLine(e.Message);

                            Environment.Exit(1);
                        }
                        catch (UserDoesNotExistException e)
                        {
                            Console.WriteLine(e.Message);

                            break;
                        }
                        flag = false;
                        break;
                    case "2":
                    case "REGISTER":
                    case "R":
                        UserRegisterImpl.StartRegister();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Something went wrong. Try again");
                        break;
                }

            } while (flag);

            flag = true;

            if (loggedUser.userName.Equals("ADMIN") && loggedUser.userId == 0)
            {
                await Menu.MenuForAdminAsync(loggedUser);
            }
            else { 
                await Menu.MenuForUserAsync(loggedUser);
            }
        }
    }

}
    

