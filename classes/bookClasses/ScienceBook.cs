using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.abstracts;

namespace LibraryProject.classes.bookClasses
{
    internal class ScienceBook : BookAbstract
    {

        public ScienceBook() : base("N/A", "N/A", 9999999999999, "Science") { }

        public ScienceBook(string title, string author, long isbn) : base(title, author, isbn, "Science")
        {

        }

        public override void printDetailsOfBook()
        {
            base.printDetailsOfBook();
            Console.WriteLine(" - " + GetType().Name);
        }
    }
}
