using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;

namespace LibraryProject.classes;


// G��wna klasa systemu biblioteki
public class LibrarySystem
{
    private static readonly string BooksFile = @"../../../libraries/books.json";


    private const string UsersFile = "classes/users.json"; // �cie�ka do pliku z u�ytkownikami

   

    // Funkcja do wczytania ksi��ek z pliku JSON
    public static async Task<List<DataBaseBook>> LoadBooksAsync()
    {
    //diagnostyka
        //Console.WriteLine($"Sprawdzam �cie�k� do pliku: {BooksFile}");
        //Console.WriteLine($"Czy plik istnieje: {File.Exists(BooksFile)}");

        if (!File.Exists(BooksFile)) return new List<DataBaseBook>(); // Je�li plik nie istnieje, zwracamy pust� list�
        string json = await File.ReadAllTextAsync(BooksFile); // Wczytanie zawarto�ci pliku
        return JsonSerializer.Deserialize<List<DataBaseBook>>(json) ?? new List<DataBaseBook>(); // Deserializacja JSON na list� obiekt�w Book
    }

    //// Funkcja do wczytania u�ytkownik�w z pliku JSON
    //public static async Task<List<User>> LibrarySystem.LoadUsersAsync()
    //{
    //    if (!File.Exists(UsersFile)) return new List<User>(); // Je�li plik nie istnieje, zwracamy pust� list�
    //    string json = await File.ReadAllTextAsync(UsersFile); // Wczytanie zawarto�ci pliku
    //    return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>(); // Deserializacja JSON na list� obiekt�w User
    //}

    // Funkcja do zapisu ksi��ek do pliku JSON
    public static async Task SaveBooksAsync(List<DataBaseBook> books)
    {
        string json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true }); // Serializacja do JSON
        await File.WriteAllTextAsync(BooksFile, json); // Zapis do pliku
    }

};