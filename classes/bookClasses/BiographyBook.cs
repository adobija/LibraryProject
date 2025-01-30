using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.abstracts;

namespace LibraryProject.classes.bookClasses
{
    internal class BiographyBook : BookAbstract
    {

        public BiographyBook() : base("N/A", "N/A", 9999999999999, "Biography") { }

        public BiographyBook(string title, string author, long isbn) : base(title, author, isbn, "Biography")
        {

        }

        public override void printDetailsOfBook()
        {
            base.printDetailsOfBook();
            Console.WriteLine(" - " + GetType().Name);
        }
    }
}
