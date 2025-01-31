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
using LibraryProject.classes;
using LibraryProject.libraries;



namespace LibraryProject.controllers;

// Klasa obsługująca funkcje użytkownika
public static class LibraryActions
{

    // do testowania zczytywania danych z pliku JSON
    public static async Task<List<DataBaseBook>> LoadBooksAsync()
    {
        // Zmieniamy ścieżkę pliku na odpowiednią dla lokalizacji w katalogu projektu
        string fullPath = "../../../libraries/books.json";

        // Diagnostyka ścieżki pliku
        //Console.WriteLine($"Ścieżka do pliku books.json: {fullPath}");

        if (!File.Exists(fullPath))
        {
            Console.WriteLine("Plik books.json nie istnieje w podanej ścieżce.");
            return new List<DataBaseBook>(); // Zwracamy pustą listę, gdy plik nie istnieje
        }

        try
        {
            // Wczytanie zawartości pliku
            string json = await File.ReadAllTextAsync(fullPath);

            // Diagnostyka zawartości pliku
            //Console.WriteLine($"Zawartość pliku JSON: {json}");

            // Deserializacja JSON do listy książek
            var books = JsonSerializer.Deserialize<List<DataBaseBook>>(json);

            if (books == null || books.Count == 0)
            {
                Console.WriteLine("Plik books.json jest pusty lub zawiera nieprawidłowe dane.");
                return new List<DataBaseBook>();
            }

            return books;
        }
        catch (Exception ex)
        {
            // Obsługa błędów
            Console.WriteLine($"Błąd podczas wczytywania książek: {ex.Message}");
            return new List<DataBaseBook>();
        }
    }




    // Przeglądanie książek z podziałem na kategorie
    public static async Task BrowseBooksAsync()
    {
        // Wczytanie książek z pliku
        var books = await LibrarySystem.LoadBooksAsync();

        // Pobierz unikalne kategorie
        var categories = books.Select(b => b.Category).Distinct().ToList();

        Console.WriteLine("Wybierz kategorię książek:");
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

            Console.WriteLine($"Książki w kategorii: {selectedCategory}");
            foreach (var book in filteredBooks)
            {
                string availability = book.IsAvailable ? "Dostępna" : "Wypożyczona";
                Console.WriteLine($"ID: {book.ISBN}, Tytuł: {book.Title}, Autor: {book.Author}, Dostępność: {availability}");
            }
        }
        else
        {
            Console.WriteLine("Niepoprawny wybór kategorii.");
        }
    }



    // Wypożyczanie książki z wyborem kategorii , wskazanie tytułu i autora ksiązki
    public static async Task BorrowBookAsync(User loggedUser)
    {
        var books = await LibrarySystem.LoadBooksAsync();
        var categories = books.Select(b => b.Category).Distinct().ToList();

        Console.WriteLine("Wybierz kategorię książek:");
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
                Console.WriteLine($"Dostępne książki w kategorii: {selectedCategory}");
                foreach (var book in availableBooks)
                {
                    Console.WriteLine($"Tytuł: {book.Title}, Autor: {book.Author}");
                }

                string title = "";
                string author = "";
                DataBaseBook bookToBorrow = null;

                while (bookToBorrow == null)
                {
                    Console.Write("Podaj tytuł książki, którą chcesz wypożyczyć: ");
                    title = Console.ReadLine();
                    bookToBorrow = availableBooks.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

                    if (bookToBorrow == null)
                    {
                        Console.WriteLine("Nie znaleziono książki o podanym tytule. Spróbuj ponownie.");
                    }
                }

                while (true)
                {
                    Console.Write("Podaj autora książki: ");
                    author = Console.ReadLine();

                    if (bookToBorrow.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                    {
                        // Aktualizuj książkę jako niedostępną
                        bookToBorrow.IsAvailable = false;
                        bookToBorrow.plusBorrowCount();

                        var borrowedBook = new BorrowedBook(bookToBorrow.ISBN, DateTime.Now);
                        loggedUser.BorrowedBooks.Add(borrowedBook);

                        // Zapisz zmiany w książkach
                        await LibrarySystem.SaveBooksAsync(books);

                        // Zapisz zmiany w użytkownikach
                        await UserController.SaveUpdatedUser(loggedUser);

                        Console.WriteLine($"Książka '{bookToBorrow.Title}' została wypożyczona.");

                        break;
                    }
                    else
                    {
                        Console.WriteLine("Autor książki jest nieprawidłowy. Spróbuj ponownie.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Brak dostępnych książek w kategorii: {selectedCategory}");
            }
        }
        else
        {
            Console.WriteLine("Niepoprawny wybór kategorii.");
        }
    }




    public static async Task ReturnBookAsync(User loggedUser)
    {
        var books = await LibrarySystem.LoadBooksAsync();

        // Pobierz unikalne kategorie książek
        var categories = books.Select(b => b.Category).Distinct().ToList();

        Console.WriteLine("Wybierz kategorię książek:");
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i]}");
        }

        Console.Write("Podaj numer kategorii: ");
        if (int.TryParse(Console.ReadLine(), out int categoryChoice) &&
            categoryChoice > 0 && categoryChoice <= categories.Count)
        {
            string selectedCategory = categories[categoryChoice - 1];

            // Filtruj książki po kategorii i wypożyczonych
            var borrowedBooks = books
                .Where(b => b.Category == selectedCategory && !b.IsAvailable)
                .ToList();

            if (borrowedBooks.Any())
            {
                Console.WriteLine($"Wypożyczone książki w kategorii: {selectedCategory}");
                ListPrint.printList(borrowedBooks);

                string title = "";
                string author = "";
                DataBaseBook bookToReturn = null;

                // Pętla dla tytułu książki
                while (bookToReturn == null)
                {
                    Console.Write("Podaj tytuł książki, którą chcesz zwrócić: ");
                    title = Console.ReadLine();

                    // Znajdź książkę na podstawie tytułu
                    bookToReturn = borrowedBooks.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

                    if (bookToReturn == null)
                    {
                        Console.WriteLine("Nie znaleziono książki o podanym tytule. Spróbuj ponownie.");
                    }
                }

                // Pętla dla autora książki
                while (bookToReturn != null)
                {
                    Console.Write("Podaj autora książki: ");
                    author = Console.ReadLine();

                    // Sprawdź, czy autor zgadza się z książką
                    if (bookToReturn.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                    {
                        // Oznaczenie książki jako dostępnej
                        bookToReturn.IsAvailable = true;
                        int index = 0;
                        foreach (DataBaseBook book in borrowedBooks) {
                            if (book.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && book.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                            {
                                break;
                            }
                            else {
                                index++;
                            }
                        } 

                        loggedUser.BorrowedBooks.RemoveAt(index);

                        // Zapisanie zmian
                        await LibrarySystem.SaveBooksAsync(books);

                        // Zapisz zmiany w użytkownikach
                        await UserController.SaveUpdatedUser(loggedUser);

                        Console.WriteLine($"Książka '{bookToReturn.Title}' została zwrócona i jest teraz dostępna do wypożyczenia.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Autor książki jest nieprawidłowy. Spróbuj ponownie.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Brak wypożyczonych książek w kategorii: {selectedCategory}");
            }
        }
        else
        {
            Console.WriteLine("Niepoprawny wybór kategorii.");
        }
    }


    public static async Task DisplayBorrowedBooksPanelAsync(User loggedUser)
    {
        try
        {
            if (loggedUser.BorrowedBooks.Count == 0)
            {
                Console.WriteLine($" {loggedUser.userName}, nie masz wypożyczonych książek.");
                return;
            }

            var books = await LibrarySystem.LoadBooksAsync();
            if (books == null || books.Count == 0)
            {
                Console.WriteLine(" Brak danych o książkach.");
                return;
            }

            // Pobranie wypożyczonych książek użytkownika z pełnymi danymi
            var borrowedBooksDetails = loggedUser.BorrowedBooks
                .Select(borrowed => books.FirstOrDefault(book => book.ISBN == borrowed.BookId))
                .Where(book => book != null)
                .ToList();

            if (borrowedBooksDetails.Count == 0)
            {
                Console.WriteLine(" Brak szczegółowych danych o wypożyczonych książkach.");
                return;
            }

            // Grupowanie książek według kategorii
            var booksByCategory = borrowedBooksDetails
                .GroupBy(book => book.Category)
                .ToDictionary(group => group.Key, group => group.ToList());

            // Wyświetlenie panelu
            Console.WriteLine($"\n Panel wypożyczeń dla użytkownika: {loggedUser.userName}");
            Console.WriteLine(new string('-', 50));

            foreach (var category in booksByCategory)
            {
                Console.WriteLine($"\n ==========================================  Kategoria: {category.Key} ==========================================");
                Console.WriteLine(new string('-', 50));

                foreach (var book in category.Value)
                {
                    Console.WriteLine($" Tytuł: {book.Title} ");
                    Console.WriteLine($" Autor: {book.Author}");
                    Console.WriteLine($" ISBN: {book.ISBN}");
                    Console.WriteLine($" Data wypożyczenia: {loggedUser.BorrowedBooks.First(b => b.BookId == book.ISBN).BorrowDate:dd-MM-yyyy}");
                    Console.WriteLine(new string('-', 50));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Wystąpił błąd: {ex.Message}");
        }
    }

}



