using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.abstracts;

namespace LibraryProject.classes.bookClasses
{
    internal class AdventureBook : BookAbstract
    {

        public AdventureBook() : base("N/A", "N/A", 9999999999999, "Adventure") { }

        public AdventureBook(string title, string author, long isbn) : base(title, author, isbn, "Adventure")
        {

        }

        public override void printDetailsOfBook()
        {
            base.printDetailsOfBook();
            Console.WriteLine(" - " + GetType().Name);
        }
    }
}
