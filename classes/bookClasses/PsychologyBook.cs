using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.abstracts;

namespace LibraryProject.classes.bookClasses
{
    internal class PsychologyBook : BookAbstract
    {

        public PsychologyBook() : base("N/A", "N/A", 9999999999999, "Psychology") { }

        public PsychologyBook(string title, string author, long isbn) : base(title, author, isbn, "Psychology")
        {

        }

        public override void printDetailsOfBook()
        {
            base.printDetailsOfBook();
            Console.WriteLine(" - " + GetType().Name);
        }
    }
}
