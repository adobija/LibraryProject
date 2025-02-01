using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.classes;
using LibraryProject.controllers;

namespace LibraryProject.libraries
{
    internal class Menu
    {
        public static async Task MenuForUserAsync(User loggedUser) {
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

        public static async Task MenuForAdminAsync(User loggedUser)
        {
            bool exit = false;
            Console.Clear(); // Czyszczenie ekranu
            Console.WriteLine("Witaj w systemie zarządzania biblioteką!");
            Console.WriteLine($"\nZalogowany użytkownik: {loggedUser.userName}");
            while (!exit)
            {
                Console.WriteLine("\nWybierz akcję:");
                Console.WriteLine("1. Przeglądaj książki");
                Console.WriteLine("2. Wypożycz książkę");
                Console.WriteLine("3. Zwróć książkę");
                Console.WriteLine("4. Wyświetl panel wypożyczeń");
                Console.WriteLine("5. Wygeneruj wykres statystyk dotyczące wypożyczeń");
                Console.WriteLine("6. Dodaj nową książkę do systemu");
                Console.WriteLine("7. Usuń istniejącą ksiązkę z systemu");
                Console.WriteLine("8. Wyjdź");

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
                    case "3":
                        await LibraryActions.ReturnBookAsync(loggedUser);
                        break;
                    case "4":
                        await LibraryActions.DisplayBorrowedBooksPanelAsync(loggedUser);
                        break;
                    case "5":
                        await BarChartGenerator.generate();
                        break;
                    case "6":
                        await LibrarySystem.addBookToSystemAsync(loggedUser);
                        break;
                    case "7":
                        await LibrarySystem.removeBookAsync(loggedUser);
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
