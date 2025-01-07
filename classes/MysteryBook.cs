using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.abstracts;

namespace LibraryProject.classes
{
    internal class MysteryBook : BookAbstract
    {

        public MysteryBook() : base("N/A", "N/A", 9999999999999) { }

        public MysteryBook(string title, string author, int isbn) : base(title, author, isbn)
        {

        }
    }
}
