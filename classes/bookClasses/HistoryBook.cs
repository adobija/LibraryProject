﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.abstracts;

namespace LibraryProject.classes.bookClasses
{
    internal class HistoryBook : BookAbstract
    {

        public HistoryBook() : base("N/A", "N/A", 9999999999999, "History") { }

        public HistoryBook(string title, string author, long isbn) : base(title, author, isbn, "History")
        {

        }

        public override void printDetailsOfBook()
        {
            base.printDetailsOfBook();
            Console.WriteLine(" - " + GetType().Name);
        }
    }
}
