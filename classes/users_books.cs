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
    public required string Title { get; set; } = string.Empty; // Tytu³ jest wymagany
    public required string Author { get; set; } = string.Empty; // Autor jest wymagany
    public required string Category { get; set; } = string.Empty; // Kategoria jest wymagana
    public bool IsAvailable { get; set; } = true; // Domyœlnie ksi¹¿ka jest dostêpna
}

// Klasa reprezentuj¹ca wypo¿yczenie ksi¹¿ki
public class BorrowedBook
{
    public int BookId { get; set; } // Identyfikator ksi¹¿ki
    public DateTime BorrowDate { get; set; } // Data wypo¿yczenia
}

// Klasa reprezentuj¹ca u¿ytkownika
public class User
{
    public int UserId { get; set; }
    public required string Name { get; set; } = string.Empty; // Imiê i nazwisko wymagane
    public List<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>(); // Domyœlnie pusta lista
}
// G³ówna klasa systemu biblioteki
public class LibrarySystem
{
    private static readonly string BooksFile = @"../../../libraries/books.json";


    private const string UsersFile = "classes/users.json"; // Œcie¿ka do pliku z u¿ytkownikami

   

    // Funkcja do wczytania ksi¹¿ek z pliku JSON
    public static async Task<List<Book>> LoadBooksAsync()
    {
    //diagnostyka
        //Console.WriteLine($"Sprawdzam œcie¿kê do pliku: {BooksFile}");
        //Console.WriteLine($"Czy plik istnieje: {File.Exists(BooksFile)}");

        if (!File.Exists(BooksFile)) return new List<Book>(); // Jeœli plik nie istnieje, zwracamy pust¹ listê
        string json = await File.ReadAllTextAsync(BooksFile); // Wczytanie zawartoœci pliku
        return JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>(); // Deserializacja JSON na listê obiektów Book
    }

    // Funkcja do wczytania u¿ytkowników z pliku JSON
    public static async Task<List<User>> LoadUsersAsync()
    {
        if (!File.Exists(UsersFile)) return new List<User>(); // Jeœli plik nie istnieje, zwracamy pust¹ listê
        string json = await File.ReadAllTextAsync(UsersFile); // Wczytanie zawartoœci pliku
        return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>(); // Deserializacja JSON na listê obiektów User
    }

    // Funkcja do zapisu ksi¹¿ek do pliku JSON
    public static async Task SaveBooksAsync(List<Book> books)
    {
        string json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true }); // Serializacja do JSON
        await File.WriteAllTextAsync(BooksFile, json); // Zapis do pliku
    }

    // Funkcja do zapisu u¿ytkowników do pliku JSON
    public static async Task SaveUsersAsync(List<User> users)
    {
        string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }); // Serializacja do JSON
        await File.WriteAllTextAsync(UsersFile, json); // Zapis do pliku
    }
};