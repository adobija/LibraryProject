using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.classes
{
    public class BorrowedBook 
    {
        public long BookId { get; set; } // Identyfikator książki
        public DateTime BorrowDate { get; set; } // Data wypożyczenia

        public BorrowedBook(long BookId, DateTime BorrowDate) { }
    }
}
