using LibraryProject.classes;

namespace LibraryProject
{
    internal class Program
    {
        static void Main(string[] args)
        {

            FantasyBook wiedzmin = new FantasyBook();

            wiedzmin.printDetailsOfBook();

            RomanceNovelBook nudy = new RomanceNovelBook("Jakis tytul", "autor", 1235438370384);

            nudy.printDetailsOfBook();

        }
    }
}
