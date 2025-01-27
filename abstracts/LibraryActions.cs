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

// Klasa obs³uguj¹ca funkcje u¿ytkownika
public static class LibraryActions
{

    // do testowania zczytywania danych z pliku JSON
    public static async Task<List<DataBaseBook>> LoadBooksAsync()
    {
        // Zmieniamy œcie¿kê pliku na odpowiedni¹ dla lokalizacji w katalogu projektu
        string fullPath = "../../../libraries/books.json";

        // Diagnostyka œcie¿ki pliku
        //Console.WriteLine($"Œcie¿ka do pliku books.json: {fullPath}");

        if (!File.Exists(fullPath))
        {
            Console.WriteLine("Plik books.json nie istnieje w podanej œcie¿ce.");
            return new List<DataBaseBook>(); // Zwracamy pust¹ listê, gdy plik nie istnieje
        }

        try
        {
            // Wczytanie zawartoœci pliku
            string json = await File.ReadAllTextAsync(fullPath);

            // Diagnostyka zawartoœci pliku
            //Console.WriteLine($"Zawartoœæ pliku JSON: {json}");

            // Deserializacja JSON do listy ksi¹¿ek
            var books = JsonSerializer.Deserialize<List<DataBaseBook>>(json);

            if (books == null || books.Count == 0)
            {
                Console.WriteLine("Plik books.json jest pusty lub zawiera nieprawid³owe dane.");
                return new List<DataBaseBook>();
            }

            return books;
        }
        catch (Exception ex)
        {
            // Obs³uga b³êdów
            Console.WriteLine($"B³¹d podczas wczytywania ksi¹¿ek: {ex.Message}");
            return new List<DataBaseBook>();
        }
    }




    // Przegl¹danie ksi¹¿ek z podzia³em na kategorie
    public static async Task BrowseBooksAsync()
    {
        Console.WriteLine("Przegl¹danie ksi¹¿ek...");


        // Wczytanie ksi¹¿ek z pliku
        var books = await LibrarySystem.LoadBooksAsync();

        //// Diagnostyka: SprawdŸ, ile ksi¹¿ek zosta³o wczytanych
        //Console.WriteLine($"Liczba ksi¹¿ek wczytanych: {books.Count}");

        //if (!books.Any())
        //{
        //    Console.WriteLine("Brak dostêpnych ksi¹¿ek.");
        //    return;
        //}

        // Pobierz unikalne kategorie
        var categories = books.Select(b => b.Category).Distinct().ToList();

        Console.WriteLine("Wybierz kategoriê ksi¹¿ek:");
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

            Console.WriteLine($"Ksi¹¿ki w kategorii: {selectedCategory}");
            foreach (var book in filteredBooks)
            {
                string availability = book.IsAvailable ? "Dostêpna" : "Wypo¿yczona";
                Console.WriteLine($"ID: {book.ISBN}, Tytu³: {book.Title}, Autor: {book.Author}, Dostêpnoœæ: {availability}");
            }
        }
        else
        {
            Console.WriteLine("Niepoprawny wybór kategorii.");
        }
    }



    // Wypo¿yczanie ksi¹¿ki z wyborem kategorii , wskazanie tytu³u i autora ksi¹zki
    public static async Task BorrowBookAsync(User loggedUser)
    {
        var books = await LibrarySystem.LoadBooksAsync();
        var categories = books.Select(b => b.Category).Distinct().ToList();

        Console.WriteLine("Wybierz kategoriê ksi¹¿ek:");
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
                Console.WriteLine($"Dostêpne ksi¹¿ki w kategorii: {selectedCategory}");
                foreach (var book in availableBooks)
                {
                    Console.WriteLine($"Tytu³: {book.Title}, Autor: {book.Author}");
                }

                string title = "";
                string author = "";
                DataBaseBook bookToBorrow = null;

                while (bookToBorrow == null)
                {
                    Console.Write("Podaj tytu³ ksi¹¿ki, któr¹ chcesz wypo¿yczyæ: ");
                    title = Console.ReadLine();
                    bookToBorrow = availableBooks.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

                    if (bookToBorrow == null)
                    {
                        Console.WriteLine("Nie znaleziono ksi¹¿ki o podanym tytule. Spróbuj ponownie.");
                    }
                }

                while (true)
                {
                    Console.Write("Podaj autora ksi¹¿ki: ");
                    author = Console.ReadLine();

                    if (bookToBorrow.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                    {
                        // Aktualizuj ksi¹¿kê jako niedostêpn¹
                        bookToBorrow.IsAvailable = false;

                        var borrowedBook = new BorrowedBook(bookToBorrow.ISBN, DateTime.Now);
                        loggedUser.BorrowedBooks.Add(borrowedBook);

                        // Zapisz zmiany w ksi¹¿kach
                        await LibrarySystem.SaveBooksAsync(books);

                        // Zapisz zmiany w u¿ytkownikach
                        await UserController.SaveUpdatedUser(loggedUser);

                        Console.WriteLine($"Ksi¹¿ka '{bookToBorrow.Title}' zosta³a wypo¿yczona.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Autor ksi¹¿ki jest nieprawid³owy. Spróbuj ponownie.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Brak dostêpnych ksi¹¿ek w kategorii: {selectedCategory}");
            }
        }
        else
        {
            Console.WriteLine("Niepoprawny wybór kategorii.");
        }
    }




    public static async Task ReturnBookAsync()
    {
        var books = await LibrarySystem.LoadBooksAsync();

        // Pobierz unikalne kategorie ksi¹¿ek
        var categories = books.Select(b => b.Category).Distinct().ToList();

        Console.WriteLine("Wybierz kategoriê ksi¹¿ek:");
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i]}");
        }

        Console.Write("Podaj numer kategorii: ");
        if (int.TryParse(Console.ReadLine(), out int categoryChoice) &&
            categoryChoice > 0 && categoryChoice <= categories.Count)
        {
            string selectedCategory = categories[categoryChoice - 1];

            // Filtruj ksi¹¿ki po kategorii i wypo¿yczonych
            var borrowedBooks = books
                .Where(b => b.Category == selectedCategory && !b.IsAvailable)
                .ToList();

            if (borrowedBooks.Any())
            {
                Console.WriteLine($"Wypo¿yczone ksi¹¿ki w kategorii: {selectedCategory}");
                foreach (var book in borrowedBooks)
                {
                    Console.WriteLine($"Tytu³: {book.Title}, Autor: {book.Author}");
                }

                string title = "";
                string author = "";
                DataBaseBook bookToReturn = null;

                // Pêtla dla tytu³u ksi¹¿ki
                while (bookToReturn == null)
                {
                    Console.Write("Podaj tytu³ ksi¹¿ki, któr¹ chcesz zwróciæ: ");
                    title = Console.ReadLine();

                    // ZnajdŸ ksi¹¿kê na podstawie tytu³u
                    bookToReturn = borrowedBooks.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

                    if (bookToReturn == null)
                    {
                        Console.WriteLine("Nie znaleziono ksi¹¿ki o podanym tytule. Spróbuj ponownie.");
                    }
                }

                // Pêtla dla autora ksi¹¿ki
                while (bookToReturn != null)
                {
                    Console.Write("Podaj autora ksi¹¿ki: ");
                    author = Console.ReadLine();

                    // SprawdŸ, czy autor zgadza siê z ksi¹¿k¹
                    if (bookToReturn.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                    {
                        // Oznaczenie ksi¹¿ki jako dostêpnej
                        bookToReturn.IsAvailable = true;

                        // Zapisanie zmian
                        await LibrarySystem.SaveBooksAsync(books);

                        Console.WriteLine($"Ksi¹¿ka '{bookToReturn.Title}' zosta³a zwrócona i jest teraz dostêpna do wypo¿yczenia.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Autor ksi¹¿ki jest nieprawid³owy. Spróbuj ponownie.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Brak wypo¿yczonych ksi¹¿ek w kategorii: {selectedCategory}");
            }
        }
        else
        {
            Console.WriteLine("Niepoprawny wybór kategorii.");
        }
    }



}



