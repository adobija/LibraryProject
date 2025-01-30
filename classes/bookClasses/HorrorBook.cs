using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.abstracts;

namespace LibraryProject.classes.bookClasses
{
    internal class HorrorBook : BookAbstract
    {

        public HorrorBook() : base("N/A", "N/A", 9999999999999, "Horror") { }

        public HorrorBook(string title, string author, long isbn) : base(title, author, isbn, "Horror")
        {

        }

        public override void printDetailsOfBook()
        {
            base.printDetailsOfBook();
            Console.WriteLine(" - " + GetType().Name);
        }
    }
}
