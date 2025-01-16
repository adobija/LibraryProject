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

// Klasa obs�uguj�ca funkcje u�ytkownika
public static class LibraryActions
{

// do testowania zczytywania danych z pliku JSON

    public static async Task<List<Book>> LoadBooksAsync()
    {
        // Zmieniamy �cie�k� pliku na odpowiedni� dla lokalizacji w katalogu projektu
        string fullPath = Path.Combine("C:", "Users", "kruzl", "Desktop", "Github", "LibraryProject", "classes", "books.json");

        // Diagnostyka �cie�ki pliku
        //Console.WriteLine($"�cie�ka do pliku books.json: {fullPath}");

        if (!File.Exists(fullPath))
        {
            Console.WriteLine("Plik books.json nie istnieje w podanej �cie�ce.");
            return new List<Book>(); // Zwracamy pust� list�, gdy plik nie istnieje
        }

        try
        {
            // Wczytanie zawarto�ci pliku
            string json = await File.ReadAllTextAsync(fullPath);

            // Diagnostyka zawarto�ci pliku
            //Console.WriteLine($"Zawarto�� pliku JSON: {json}");

            // Deserializacja JSON do listy ksi��ek
            var books = JsonSerializer.Deserialize<List<Book>>(json);

            if (books == null || books.Count == 0)
            {
                Console.WriteLine("Plik books.json jest pusty lub zawiera nieprawid�owe dane.");
                return new List<Book>();
            }

            return books;
        }
        catch (Exception ex)
        {
            // Obs�uga b��d�w
            Console.WriteLine($"B��d podczas wczytywania ksi��ek: {ex.Message}");
            return new List<Book>();
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
                Console.WriteLine($"ID: {book.Id}, Tytu�: {book.Title}, Autor: {book.Author}, Dost�pno��: {availability}");
            }
        }
        else
        {
            Console.WriteLine("Niepoprawny wyb�r kategorii.");
        }
    }




    // Wypo�yczanie ksi��ki
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
            Console.WriteLine($"U�ytkownik {user.Name} wypo�yczy� ksi��k�: {book.Title}");
        }
        else
        {
            Console.WriteLine("Ksi��ka niedost�pna lub u�ytkownik nie znaleziony.");
        }
    }


    // Wypo�yczanie ksi��ki z wyborem kategorii i ID ksi��ki
    public static async Task BorrowBookAsync()
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
            var filteredBooks = books.Where(b => b.Category == selectedCategory).ToList();

            Console.WriteLine($"Ksi��ki w kategorii: {selectedCategory}");
            foreach (var book in filteredBooks)
            {
                string availability = book.IsAvailable ? "Dost�pna" : "Wypo�yczona";
                Console.WriteLine($"ID: {book.Id}, Tytu�: {book.Title}, Autor: {book.Author}, Dost�pno��: {availability}");
            }

            Console.Write("Podaj ID ksi��ki, kt�r� chcesz wypo�yczy�: ");
            if (int.TryParse(Console.ReadLine(), out int bookId))
            {
                var book = filteredBooks.FirstOrDefault(b => b.Id == bookId);

                if (book != null)
                {
                    if (book.IsAvailable)
                    {
                        // Oznaczenie ksi��ki jako niedost�pnej
                        book.IsAvailable = false;

                        // Zapisanie zmian
                        await LibrarySystem.SaveBooksAsync(books);

                        Console.WriteLine($"Ksi��ka '{book.Title}' zosta�a wypo�yczona.");
                    }
                    else
                    {
                        Console.WriteLine("Wybrana ksi��ka jest ju� wypo�yczona.");
                    }
                }
                else
                {
                    Console.WriteLine("Nie znaleziono ksi��ki o podanym ID.");
                }
            }
            else
            {
                Console.WriteLine("Nieprawid�owe ID ksi��ki.");
            }
        }
        else
        {
            Console.WriteLine("Niepoprawny wyb�r kategorii.");
        }
    }






    // Zwracanie ksi��ki
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
                Console.WriteLine($"U�ytkownik {user.Name} zwr�ci� ksi��k�: {book.Title}");
            }
            else
            {
                Console.WriteLine("U�ytkownik nie wypo�yczy� tej ksi��ki.");
            }
        }
    }

    // Panel z czasem wypo�yczenia ksi��ki
    public static async Task DisplayBorrowPanelAsync()
    {
        var users = await LibrarySystem.LoadUsersAsync();
        var books = await LibrarySystem.LoadBooksAsync();

        foreach (var user in users)
        {
            Console.WriteLine($"\nU�ytkownik: {user.Name}");
            foreach (var borrowedBook in user.BorrowedBooks)
            {
                var book = books.FirstOrDefault(b => b.Id == borrowedBook.BookId);
                if (book != null)
                {
                    var daysBorrowed = (DateTime.Now - borrowedBook.BorrowDate).Days;
                    Console.WriteLine($"ID: {book.Id}, Tytu�: {book.Title}, Autor: {book.Author}, Kategoria: {book.Category}, Dni wypo�yczenia: {daysBorrowed}");
                }
            }
        }
    }
};
