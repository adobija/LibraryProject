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



namespace LibraryProject.classes;

// Klasa obs³uguj¹ca funkcje u¿ytkownika
public static class LibraryActions
{

// do testowania zczytywania danych z pliku JSON

    public static async Task<List<Book>> LoadBooksAsync()
    {
        // Zmieniamy œcie¿kê pliku na odpowiedni¹ dla lokalizacji w katalogu projektu
        string fullPath = Path.Combine("C:", "Users", "kruzl", "Desktop", "Github", "LibraryProject", "classes", "books.json");

        // Diagnostyka œcie¿ki pliku
        //Console.WriteLine($"Œcie¿ka do pliku books.json: {fullPath}");

        if (!File.Exists(fullPath))
        {
            Console.WriteLine("Plik books.json nie istnieje w podanej œcie¿ce.");
            return new List<Book>(); // Zwracamy pust¹ listê, gdy plik nie istnieje
        }

        try
        {
            // Wczytanie zawartoœci pliku
            string json = await File.ReadAllTextAsync(fullPath);

            // Diagnostyka zawartoœci pliku
            //Console.WriteLine($"Zawartoœæ pliku JSON: {json}");

            // Deserializacja JSON do listy ksi¹¿ek
            var books = JsonSerializer.Deserialize<List<Book>>(json);

            if (books == null || books.Count == 0)
            {
                Console.WriteLine("Plik books.json jest pusty lub zawiera nieprawid³owe dane.");
                return new List<Book>();
            }

            return books;
        }
        catch (Exception ex)
        {
            // Obs³uga b³êdów
            Console.WriteLine($"B³¹d podczas wczytywania ksi¹¿ek: {ex.Message}");
            return new List<Book>();
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
                Console.WriteLine($"ID: {book.Id}, Tytu³: {book.Title}, Autor: {book.Author}, Dostêpnoœæ: {availability}");
            }
        }
        else
        {
            Console.WriteLine("Niepoprawny wybór kategorii.");
        }
    }




    // Wypo¿yczanie ksi¹¿ki
    public static async Task BorrowBookAsync(int userId, int bookId)
    {
        var users = await LibrarySystem.LoadUsersAsync();
        var books = await LibrarySystem.LoadBooksAsync();

        var user = users.FirstOrDefault(u => u.UserId == userId);
        var book = books.FirstOrDefault(b => b.Id == bookId && b.IsAvailable);

        if (user != null && book != null)
        {
            book.IsAvailable = false;
            user.BorrowedBooks.Add(new BorrowedBook { BookId = bookId, BorrowDate = DateTime.Now });
            await LibrarySystem.SaveBooksAsync(books);
            await LibrarySystem.SaveUsersAsync(users);
            Console.WriteLine($"U¿ytkownik {user.Name} wypo¿yczy³ ksi¹¿kê: {book.Title}");
        }
        else
        {
            Console.WriteLine("Ksi¹¿ka niedostêpna lub u¿ytkownik nie znaleziony.");
        }
    }


    // Wypo¿yczanie ksi¹¿ki z wyborem kategorii i ID ksi¹¿ki
    public static async Task BorrowBookAsync()
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
            var filteredBooks = books.Where(b => b.Category == selectedCategory).ToList();

            Console.WriteLine($"Ksi¹¿ki w kategorii: {selectedCategory}");
            foreach (var book in filteredBooks)
            {
                string availability = book.IsAvailable ? "Dostêpna" : "Wypo¿yczona";
                Console.WriteLine($"ID: {book.Id}, Tytu³: {book.Title}, Autor: {book.Author}, Dostêpnoœæ: {availability}");
            }

            Console.Write("Podaj ID ksi¹¿ki, któr¹ chcesz wypo¿yczyæ: ");
            if (int.TryParse(Console.ReadLine(), out int bookId))
            {
                var book = filteredBooks.FirstOrDefault(b => b.Id == bookId);

                if (book != null)
                {
                    if (book.IsAvailable)
                    {
                        // Oznaczenie ksi¹¿ki jako niedostêpnej
                        book.IsAvailable = false;

                        // Zapisanie zmian
                        await LibrarySystem.SaveBooksAsync(books);

                        Console.WriteLine($"Ksi¹¿ka '{book.Title}' zosta³a wypo¿yczona.");
                    }
                    else
                    {
                        Console.WriteLine("Wybrana ksi¹¿ka jest ju¿ wypo¿yczona.");
                    }
                }
                else
                {
                    Console.WriteLine("Nie znaleziono ksi¹¿ki o podanym ID.");
                }
            }
            else
            {
                Console.WriteLine("Nieprawid³owe ID ksi¹¿ki.");
            }
        }
        else
        {
            Console.WriteLine("Niepoprawny wybór kategorii.");
        }
    }






    // Zwracanie ksi¹¿ki
    public static async Task ReturnBookAsync(int userId, int bookId)
    {
        var users = await LibrarySystem.LoadUsersAsync();
        var books = await LibrarySystem.LoadBooksAsync();

        var user = users.FirstOrDefault(u => u.UserId == userId);
        var book = books.FirstOrDefault(b => b.Id == bookId);

        if (user != null && book != null)
        {
            var borrowedBook = user.BorrowedBooks.FirstOrDefault(b => b.BookId == bookId);
            if (borrowedBook != null)
            {
                book.IsAvailable = true;
                user.BorrowedBooks.Remove(borrowedBook);
                await LibrarySystem.SaveBooksAsync(books);
                await LibrarySystem.SaveUsersAsync(users);
                Console.WriteLine($"U¿ytkownik {user.Name} zwróci³ ksi¹¿kê: {book.Title}");
            }
            else
            {
                Console.WriteLine("U¿ytkownik nie wypo¿yczy³ tej ksi¹¿ki.");
            }
        }
    }

    // Panel z czasem wypo¿yczenia ksi¹¿ki
    public static async Task DisplayBorrowPanelAsync()
    {
        var users = await LibrarySystem.LoadUsersAsync();
        var books = await LibrarySystem.LoadBooksAsync();

        foreach (var user in users)
        {
            Console.WriteLine($"\nU¿ytkownik: {user.Name}");
            foreach (var borrowedBook in user.BorrowedBooks)
            {
                var book = books.FirstOrDefault(b => b.Id == borrowedBook.BookId);
                if (book != null)
                {
                    var daysBorrowed = (DateTime.Now - borrowedBook.BorrowDate).Days;
                    Console.WriteLine($"ID: {book.Id}, Tytu³: {book.Title}, Autor: {book.Author}, Kategoria: {book.Category}, Dni wypo¿yczenia: {daysBorrowed}");
                }
            }
        }
    }
};
