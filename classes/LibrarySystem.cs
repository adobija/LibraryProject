using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using LibraryProject.controllers;
using LibraryProject.exceptions;
using LibraryProject.libraries;

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

    //public static async Task SaveUsersAsync()
    //{
    //    string fullPath = "../../../libraries/userDB.json";
    //    var usersJson = JsonSerializer.Serialize(UserController.FetchUsers(), new JsonSerializerOptions { WriteIndented = true });

    //    await File.WriteAllTextAsync(fullPath, usersJson);
    //}
    public static async Task addBookToSystemAsync(User loggedUser) {
        if (loggedUser.userId != 0 && !loggedUser.userName.ToUpper().Equals("ADMIN")) {
            throw new PermissionException();
        }

        List<DataBaseBook> books = await LoadBooksAsync();

        Console.WriteLine("Insert category of book (Fantasy/Romance/Mystery)");
        string category = Console.ReadLine().ToUpper();

        var newBook = FactoryDesignLib.getBook(category);

        Console.WriteLine("Insert Title of Book");
        string title = Console.ReadLine();

        Console.WriteLine($"Insert Author of {title}");
        string author = Console.ReadLine();

        long ISBN;
        bool flag = true;
        do
        {
            Console.WriteLine($"Insert ISBN of {title} written by {author}");
            ISBN = long.Parse(Console.ReadLine());

            try
            {
                bool firstCondition = !ISBNValidator.isValid(ISBN);
                bool secondCondition = await ISBNValidator.isInDatabaseAsync(ISBN);

                flag = firstCondition && secondCondition;
            }
            catch (InvalidISBN e)
            {
                Console.WriteLine($"{e.Message}, try again");
            }
            catch (ISBNAlreadyExistException e)
            {
                Console.WriteLine($"{e.Message}, try again");
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error! Try Again!");
            }
            

        } while (flag);
        

        newBook.Title = title;
        newBook.ISBN = ISBN;
        newBook.Author = author;

        books.Add((DataBaseBook)newBook);

        SaveBooksAsync(books);
        Console.Write($"Successfully added ");
        newBook.printDetailsOfBook();
        
    }

};