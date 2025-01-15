using LibraryProject.classes;
using LibraryProject.controllers;

namespace LibraryProject
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //FantasyBook wiedzmin = new FantasyBook();

            //wiedzmin.printDetailsOfBook();


            //RomanceNovelBook nudy = new RomanceNovelBook("Jakis tytul", "autor", 1235438370384);

            //nudy.printDetailsOfBook();

            UserController userController = new UserController();

            string username = "Admin";
            string password = "P@$$w0rd";

            userController.Register(username, password);


        }
    }
}
