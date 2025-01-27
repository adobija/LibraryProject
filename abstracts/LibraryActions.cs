using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.exceptions;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using LibraryProject.controllers;



namespace LibraryProject.classes;

// Klasa obs�uguj�ca funkcje u�ytkownika
public static class LibraryActions
{

    // do testowania zczytywania danych z pliku JSON
    public static async Task<List<DataBaseBook>> LoadBooksAsync()
    {
        // Zmieniamy �cie�k� pliku na odpowiedni� dla lokalizacji w katalogu projektu
        string fullPath = "../../../libraries/books.json";

        // Diagnostyka �cie�ki pliku
        //Console.WriteLine($"�cie�ka do pliku books.json: {fullPath}");

        if (!File.Exists(fullPath))
        {
            Console.WriteLine("Plik books.json nie istnieje w podanej �cie�ce.");
            return new List<DataBaseBook>(); // Zwracamy pust� list�, gdy plik nie istnieje
        }

        try
        {
            // Wczytanie zawarto�ci pliku
            string json = await File.ReadAllTextAsync(fullPath);

            // Diagnostyka zawarto�ci pliku
            //Console.WriteLine($"Zawarto�� pliku JSON: {json}");

            // Deserializacja JSON do listy ksi��ek
            var books = JsonSerializer.Deserialize<List<DataBaseBook>>(json);

            if (books == null || books.Count == 0)
            {
                Console.WriteLine("Plik books.json jest pusty lub zawiera nieprawid�owe dane.");
                return new List<DataBaseBook>();
            }

            return books;
        }
        catch (Exception ex)
        {
            // Obs�uga b��d�w
            Console.WriteLine($"B��d podczas wczytywania ksi��ek: {ex.Message}");
            return new List<DataBaseBook>();
        }
    }




    // Przegl�danie ksi��ek z podzia�em na kategorie
    public static async Task BrowseBooksAsync()
    {
        Console.WriteLine("Przegl�danie ksi��ek...");


        // Wczytanie ksi��ek z pliku
        var books = await LibrarySystem.LoadBooksAsync();

        //// Diagnostyka: Sprawd�, ile ksi��ek zosta�o wczytanych
        //Console.WriteLine($"Liczba ksi��ek wczytanych: {books.Count}");

        //if (!books.Any())
        //{
        //    Console.WriteLine("Brak dost�pnych ksi��ek.");
        //    return;
        //}

        // Pobierz unikalne kategorie
        var categories = books.Select(b => b.Category).Distinct().ToList();

        Console.WriteLine("Wybierz kategori� ksi��ek:");
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i]}");
        }

        Console.Write("Podaj numer kategorii: ");
        if (int.TryParse(Console.ReadLine(), out int categoryChoice) &&
            categoryChoice > 0 && categoryChoice <= categories.Count)
        {
            string selectedCategory = categories[categoryChoice - 1];
            var filteredBooks = books.Where(b => b.Category == selectedCategory).ToList();

            Console.WriteLine($"Ksi��ki w kategorii: {selectedCategory}");
            foreach (var book in filteredBooks)
            {
                string availability = book.IsAvailable ? "Dost�pna" : "Wypo�yczona";
                Console.WriteLine($"ID: {book.ISBN}, Tytu�: {book.Title}, Autor: {book.Author}, Dost�pno��: {availability}");
            }
        }
        else
        {
            Console.WriteLine("Niepoprawny wyb�r kategorii.");
        }
    }



    // Wypo�yczanie ksi��ki z wyborem kategorii , wskazanie tytu�u i autora ksi�zki
    public static async Task BorrowBookAsync(User loggedUser)
    {
        var books = await LibrarySystem.LoadBooksAsync();
        var categories = books.Select(b => b.Category).Distinct().ToList();

        Console.WriteLine("Wybierz kategori� ksi��ek:");
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i]}");
        }

        Console.Write("Podaj numer kategorii: ");
        if (int.TryParse(Console.ReadLine(), out int categoryChoice) &&
            categoryChoice > 0 && categoryChoice <= categories.Count)
        {
            string selectedCategory = categories[categoryChoice - 1];
            var availableBooks = books
                .Where(b => b.Category == selectedCategory && b.IsAvailable)
                .ToList();

            if (availableBooks.Any())
            {
                Console.WriteLine($"Dost�pne ksi��ki w kategorii: {selectedCategory}");
                foreach (var book in availableBooks)
                {
                    Console.WriteLine($"Tytu�: {book.Title}, Autor: {book.Author}");
                }

                string title = "";
                string author = "";
                DataBaseBook bookToBorrow = null;

                while (bookToBorrow == null)
                {
                    Console.Write("Podaj tytu� ksi��ki, kt�r� chcesz wypo�yczy�: ");
                    title = Console.ReadLine();
                    bookToBorrow = availableBooks.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

                    if (bookToBorrow == null)
                    {
                        Console.WriteLine("Nie znaleziono ksi��ki o podanym tytule. Spr�buj ponownie.");
                    }
                }

                while (true)
                {
                    Console.Write("Podaj autora ksi��ki: ");
                    author = Console.ReadLine();

                    if (bookToBorrow.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                    {
                        // Aktualizuj ksi��k� jako niedost�pn�
                        bookToBorrow.IsAvailable = false;

                        var borrowedBook = new BorrowedBook(bookToBorrow.ISBN, DateTime.Now);
                        loggedUser.BorrowedBooks.Add(borrowedBook);

                        // Zapisz zmiany w ksi��kach
                        await LibrarySystem.SaveBooksAsync(books);

                        // Zapisz zmiany w u�ytkownikach
                        await UserController.SaveUpdatedUser(loggedUser);

                        Console.WriteLine($"Ksi��ka '{bookToBorrow.Title}' zosta�a wypo�yczona.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Autor ksi��ki jest nieprawid�owy. Spr�buj ponownie.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Brak dost�pnych ksi��ek w kategorii: {selectedCategory}");
            }
        }
        else
        {
            Console.WriteLine("Niepoprawny wyb�r kategorii.");
        }
    }




    public static async Task ReturnBookAsync()
    {
        var books = await LibrarySystem.LoadBooksAsync();

        // Pobierz unikalne kategorie ksi��ek
        var categories = books.Select(b => b.Category).Distinct().ToList();

        Console.WriteLine("Wybierz kategori� ksi��ek:");
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i]}");
        }

        Console.Write("Podaj numer kategorii: ");
        if (int.TryParse(Console.ReadLine(), out int categoryChoice) &&
            categoryChoice > 0 && categoryChoice <= categories.Count)
        {
            string selectedCategory = categories[categoryChoice - 1];

            // Filtruj ksi��ki po kategorii i wypo�yczonych
            var borrowedBooks = books
                .Where(b => b.Category == selectedCategory && !b.IsAvailable)
                .ToList();

            if (borrowedBooks.Any())
            {
                Console.WriteLine($"Wypo�yczone ksi��ki w kategorii: {selectedCategory}");
                foreach (var book in borrowedBooks)
                {
                    Console.WriteLine($"Tytu�: {book.Title}, Autor: {book.Author}");
                }

                string title = "";
                string author = "";
                DataBaseBook bookToReturn = null;

                // P�tla dla tytu�u ksi��ki
                while (bookToReturn == null)
                {
                    Console.Write("Podaj tytu� ksi��ki, kt�r� chcesz zwr�ci�: ");
                    title = Console.ReadLine();

                    // Znajd� ksi��k� na podstawie tytu�u
                    bookToReturn = borrowedBooks.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

                    if (bookToReturn == null)
                    {
                        Console.WriteLine("Nie znaleziono ksi��ki o podanym tytule. Spr�buj ponownie.");
                    }
                }

                // P�tla dla autora ksi��ki
                while (bookToReturn != null)
                {
                    Console.Write("Podaj autora ksi��ki: ");
                    author = Console.ReadLine();

                    // Sprawd�, czy autor zgadza si� z ksi��k�
                    if (bookToReturn.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                    {
                        // Oznaczenie ksi��ki jako dost�pnej
                        bookToReturn.IsAvailable = true;

                        // Zapisanie zmian
                        await LibrarySystem.SaveBooksAsync(books);

                        Console.WriteLine($"Ksi��ka '{bookToReturn.Title}' zosta�a zwr�cona i jest teraz dost�pna do wypo�yczenia.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Autor ksi��ki jest nieprawid�owy. Spr�buj ponownie.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Brak wypo�yczonych ksi��ek w kategorii: {selectedCategory}");
            }
        }
        else
        {
            Console.WriteLine("Niepoprawny wyb�r kategorii.");
        }
    }



}



