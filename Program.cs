using LibraryProject.classes;
using LibraryProject.controllers;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using LibraryProject.interfaces.implementations;
using LibraryProject.exceptions;


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

            bool exit = false;
            Console.Clear(); // Czyszczenie ekranu
            Console.WriteLine("Witaj w systemie biblioteki!");
            Console.WriteLine($"\nZalogowany użytkownik: {loggedUser.userName}");
            while (!exit)
            {
                Console.WriteLine("\nWybierz akcję:");
                Console.WriteLine("1. Przeglądaj książki");
                Console.WriteLine("2. Wypożycz książkę");
                Console.WriteLine("3. Zwróć książkę");
                Console.WriteLine("4. Wyświetl panel wypożyczeń");
                Console.WriteLine("5. Wygeneruj wykres statystyk dotyczące wypożyczeń");
                Console.WriteLine("6. Wyjdź");

                Console.Write("Twój wybór: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        await LibraryActions.BrowseBooksAsync();
                        break;


                    case "2":
                        await LibraryActions.BorrowBookAsync(loggedUser);
                        break;
                    //e69b14d4f33fe7e9ac1a35ef4f87df635d7073f0

                    case "3":

                        await LibraryActions.ReturnBookAsync(loggedUser);
                        break;

                    case "4":
                        await LibraryActions.DisplayBorrowedBooksPanelAsync(loggedUser);
                        break;
                    case "5":
                        await BarChartGenerator.generate();
                        break;

                    default:
                        Environment.Exit(0);
                        exit = true;
                        break;
                }
            }
        }
    }

}
    

