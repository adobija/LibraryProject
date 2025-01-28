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
using System.Runtime.InteropServices;
using LibraryProject.abstracts;

namespace LibraryProject.classes;


// G³ówna klasa systemu biblioteki
public class LibrarySystem
{
    private static readonly string BooksFile = @"../../../libraries/books.json";


    private const string UsersFile = "classes/users.json"; // Œcie¿ka do pliku z u¿ytkownikami

   

    // Funkcja do wczytania ksi¹¿ek z pliku JSON
    public static async Task<List<DataBaseBook>> LoadBooksAsync()
    {
    //diagnostyka
        //Console.WriteLine($"Sprawdzam œcie¿kê do pliku: {BooksFile}");
        //Console.WriteLine($"Czy plik istnieje: {File.Exists(BooksFile)}");

        if (!File.Exists(BooksFile)) return new List<DataBaseBook>(); // Jeœli plik nie istnieje, zwracamy pust¹ listê
        string json = await File.ReadAllTextAsync(BooksFile); // Wczytanie zawartoœci pliku
        return JsonSerializer.Deserialize<List<DataBaseBook>>(json) ?? new List<DataBaseBook>(); // Deserializacja JSON na listê obiektów Book
    }

    //// Funkcja do wczytania u¿ytkowników z pliku JSON
    //public static async Task<List<User>> LibrarySystem.LoadUsersAsync()
    //{
    //    if (!File.Exists(UsersFile)) return new List<User>(); // Jeœli plik nie istnieje, zwracamy pust¹ listê
    //    string json = await File.ReadAllTextAsync(UsersFile); // Wczytanie zawartoœci pliku
    //    return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>(); // Deserializacja JSON na listê obiektów User
    //}

    // Funkcja do zapisu ksi¹¿ek do pliku JSON
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
    public static async Task<bool> addBookToSystemAsync(User loggedUser) {
        if (loggedUser.userId != 0 && !loggedUser.userName.ToUpper().Equals("ADMIN")) {
            return false;
        }

        List<DataBaseBook> books = await LoadBooksAsync();

        Console.WriteLine("Insert category of book (Fantasy/Romance/Mystery)");
        string category = Console.ReadLine().ToUpper();
        BookAbstract newBook;
        try
        {
            newBook = FactoryDesignLib.getBook(category);
        }
        catch (NotImplementedException e){ 
            Console.WriteLine(e.Message);
            return false;
        }
        

        Console.WriteLine("Insert Title of Book");
        string title = Console.ReadLine();

        Console.WriteLine($"Insert Author of {title}");
        string author = Console.ReadLine();

        long ISBN = 0L;
        bool flag = true;
        do
        {
            
            Console.WriteLine($"Insert ISBN of {title} written by {author}");
            try
            {
                ISBN = long.Parse(Console.ReadLine());
            }
            catch (Exception e) {
                Console.WriteLine("Unexpected error! Try Again!");
            }
            

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

        DataBaseBook refactoredBook = new DataBaseBook(newBook.Title, newBook.Author, newBook.ISBN, newBook.Category, true);

        books.Add(refactoredBook);

        SaveBooksAsync(books);
        Console.Write($"Successfully added ");
        newBook.printDetailsOfBook();
        Console.WriteLine();

        return true;


    }

    public static async Task<bool> removeBookAsync(User loggedUser) {
        if (loggedUser.userId != 0 && !loggedUser.userName.ToUpper().Equals("ADMIN"))
        {
            return false;
        }

        List<DataBaseBook> books = await LoadBooksAsync();
        bool flag = false;
        do
        {
            Console.Clear();
            Console.WriteLine("Choose book to remove:");
            int i = 0;
            foreach (DataBaseBook book in books)
            {
                Console.Write($"{i}: ");
                book.printDetailsOfBook();
                Console.WriteLine();
                i++;
            }
            int input;
            try {
                input = int.Parse(Console.ReadLine());
            } catch (Exception e) {
                Console.WriteLine("Unexpected error! Try Again!");
                continue;
            }
            

            if (input > i) {
                Console.WriteLine("This index does not exist! Try again!");
                continue;
            }

            DataBaseBook bookToRemove = books[input];
            Console.WriteLine($"Do you want remove - ");
            bookToRemove.printDetailsOfBook();
            Console.WriteLine("?");
            Console.WriteLine("Yes [Y]/No [N]");
            
            string consent = Console.ReadLine().ToUpper();

            if (consent.Equals("YES") || consent.Equals("Y"))
            {
                books.RemoveAt(input);
                Console.WriteLine($"Successfully removed ");
                bookToRemove.printDetailsOfBook();
                Console.WriteLine();
                flag = false;
            }
            else {
                Console.WriteLine("Action not approved");
                return false;
            }

            SaveBooksAsync( books );

        } while (flag);
        return true;

        
    }

};