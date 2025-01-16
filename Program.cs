using LibraryProject.classes;
using LibraryProject.controllers;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using LibraryProject.classes;


namespace LibraryProject
{
    internal class Program
    {
        static async Task Main(string[] args)
        {


            //FantasyBook wiedzmin = new FantasyBook();


            FantasyBook wiedzmin = new FantasyBook();

            //wiedzmin.printDetailsOfBook();


            //RomanceNovelBook nudy = new RomanceNovelBook("Jakis tytul", "autor", 1235438370384);

            //nudy.printDetailsOfBook();

            UserController userController = new UserController();

            string username = "Admin";
            string password = "P@$$w0rd";

            userController.Register(username, password);




            Console.WriteLine($"Aktualna ścieżka robocza: {Directory.GetCurrentDirectory()}");


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
                        await LibraryActions.BrowseBooksAsync();
                        break;


                      case "2":

                           Console.Write("Podaj ID książki, którą chcesz wypożyczyć: ");
                           await LibraryActions.BorrowBookAsync
                        break;


                    case "3":
                        Console.Write("\nPodaj swoje ID użytkownika: ");
                        if (int.TryParse(Console.ReadLine(), out int returnUserId))
                        {
                            Console.Write("Podaj ID książki, którą chcesz zwrócić: ");
                            if (int.TryParse(Console.ReadLine(), out int returnBookId))
                            {
                                await LibraryActions.ReturnBookAsync(returnUserId, returnBookId);
                            }
                            else
                            {
                                Console.WriteLine("Nieprawidłowe ID książki.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nieprawidłowe ID użytkownika.");
                        }
                        break;

                    case "4":
                        Console.WriteLine("\nWyświetlanie panelu wypożyczeń...");
                        await LibraryActions.DisplayBorrowPanelAsync();
                        break;

                    case "5":
                        Console.WriteLine("Dziękujemy za skorzystanie z systemu biblioteki. Do widzenia!");
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                        break;
                }
            }
        }
    }



}
    

