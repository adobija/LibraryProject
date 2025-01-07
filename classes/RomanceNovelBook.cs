using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.abstracts;

namespace LibraryProject.classes
{
    internal class RomanceNovelBook : BookAbstract
    {

        public RomanceNovelBook() : base("N/A", "N/A", 9999999999999) { }

        public RomanceNovelBook(string title, string author, int isbn) : base(title, author, isbn)
        {

        }

    }
}
