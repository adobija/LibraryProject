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
            do {
                
                Console.WriteLine("1: Login");
                Console.WriteLine("2: Register");
                string response = Console.ReadLine().ToUpper();
                Console.Clear();
                
                switch (response) {
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

            Console.Clear();
            Console.WriteLine($"Welcome {loggedUser.userName.ToLowerInvariant()} \n");



            //await BarChartGenerator.generate();
            //bool flag = true;
            //do
            //{
            //    User loggedUser = null;

            //try
            //{
            //    loggedUser = await UserLoginImpl.Login();
            //}
            //catch (InvalidPasswordException e)
            //{
            //    Console.WriteLine(e.Message);

            //    Environment.Exit(1);
            //}
            //catch (UserDoesNotExistException e)
            //{
            //    Console.WriteLine(e.Message);

            //    Environment.Exit(1);
            //}

            //    Console.WriteLine("1. remove 2. add");
            //    int input = int.Parse(Console.ReadLine());
            //    if (input == 1)
            //    {
            //        flag = await LibrarySystem.removeBookAsync(loggedUser);
            //        if (!flag)
            //        {
            //            Console.WriteLine("You don't have permission to do that!");
            //        }
            //    }
            //    else {
            //        flag = await LibrarySystem.addBookToSystemAsync(loggedUser);
            //        if (!flag)
            //        {
            //            Console.WriteLine("You don't have permission to do that!");
            //        }
            //    }






            //} while (!flag);



            // e69b14d4f33fe7e9ac1a35ef4f87df635d7073f0


            //Console.WriteLine($"Logged as {loggedUser.getUserName()}");

            //UserRegisterImpl.StartRegister();






            //Console.WriteLine($"Aktualna ścieżka robocza: {Directory.GetCurrentDirectory()}");


            // Do testowania zczytywania danych z pliku books.JSON

            // Wywołanie diagnostyki LoadBooksAsync
            //Console.WriteLine("Rozpoczynamy diagnostykę LoadBooksAsync...");

            // Wywołanie metody LoadBooksAsync
            //var books = await LibrarySystem.LoadBooksAsync();

            // Diagnostyka - sprawdzenie liczby książek
            //Console.WriteLine($"Liczba książek wczytanych: {books.Count}");

            // Jeśli książki zostały wczytane, wyświetlamy ich dane
            //if (books.Any())
            //{
            //    Console.WriteLine("Książki wczytane:");
            //    foreach (var book in books)
            //    {
            //        Console.WriteLine($"ID: {book.Id}, Tytuł: {book.Title}, Autor: {book.Author}, Kategoria: {book.Category}");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Brak książek wczytanych z pliku.");
            //}

            // Kontynuacja aplikacji, np. wywołanie opcji przeglądania książek
            //await LibraryActions.BrowseBooksAsync();  // Wywołanie funkcji przeglądania książek



            Console.WriteLine("Witaj w systemie biblioteki!");
            bool exit = false;
            Console.Clear(); // Czyszczenie ekranu
            Console.WriteLine($"\nZalogowany użytkownik: {loggedUser.userName}");
            while (!exit)
            {
                Console.WriteLine("\nWybierz akcję:");
                Console.WriteLine("1. Przeglądaj książki");
                Console.WriteLine("2. Wypożycz książkę");
                Console.WriteLine("3. Zwróć książkę");
                Console.WriteLine("4. Wyświetl panel wypożyczeń");
                Console.WriteLine("5. Wyjdź");

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
                        await LibraryActions.DisplayBorrowedPanelAsync(loggedUser);
                        break;

                    default:
                        exit = true;
                        break;
                }
            }
        }
    }

}
    

