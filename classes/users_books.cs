using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;

namespace LibraryProject.classes;

public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; } = string.Empty; // Tytu� jest wymagany
    public required string Author { get; set; } = string.Empty; // Autor jest wymagany
    public required string Category { get; set; } = string.Empty; // Kategoria jest wymagana
    public bool IsAvailable { get; set; } = true; // Domy�lnie ksi��ka jest dost�pna
}

// Klasa reprezentuj�ca wypo�yczenie ksi��ki
public class BorrowedBook
{
    public int BookId { get; set; } // Identyfikator ksi��ki
    public DateTime BorrowDate { get; set; } // Data wypo�yczenia
}

// Klasa reprezentuj�ca u�ytkownika
public class User
{
    public int UserId { get; set; }
    public required string Name { get; set; } = string.Empty; // Imi� i nazwisko wymagane
    public List<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>(); // Domy�lnie pusta lista
}
// G��wna klasa systemu biblioteki
public class LibrarySystem
{
    private static readonly string BooksFile = @"../../../libraries/books.json";


    private const string UsersFile = "classes/users.json"; // �cie�ka do pliku z u�ytkownikami

   

    // Funkcja do wczytania ksi��ek z pliku JSON
    public static async Task<List<Book>> LoadBooksAsync()
    {
    //diagnostyka
        //Console.WriteLine($"Sprawdzam �cie�k� do pliku: {BooksFile}");
        //Console.WriteLine($"Czy plik istnieje: {File.Exists(BooksFile)}");

        if (!File.Exists(BooksFile)) return new List<Book>(); // Je�li plik nie istnieje, zwracamy pust� list�
        string json = await File.ReadAllTextAsync(BooksFile); // Wczytanie zawarto�ci pliku
        return JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>(); // Deserializacja JSON na list� obiekt�w Book
    }

    // Funkcja do wczytania u�ytkownik�w z pliku JSON
    public static async Task<List<User>> LoadUsersAsync()
    {
        if (!File.Exists(UsersFile)) return new List<User>(); // Je�li plik nie istnieje, zwracamy pust� list�
        string json = await File.ReadAllTextAsync(UsersFile); // Wczytanie zawarto�ci pliku
        return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>(); // Deserializacja JSON na list� obiekt�w User
    }

    // Funkcja do zapisu ksi��ek do pliku JSON
    public static async Task SaveBooksAsync(List<Book> books)
    {
        string json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true }); // Serializacja do JSON
        await File.WriteAllTextAsync(BooksFile, json); // Zapis do pliku
    }

    // Funkcja do zapisu u�ytkownik�w do pliku JSON
    public static async Task SaveUsersAsync(List<User> users)
    {
        string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }); // Serializacja do JSON
        await File.WriteAllTextAsync(UsersFile, json); // Zapis do pliku
    }
};