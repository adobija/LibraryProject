using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.classes;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryProject.libraries
{
    internal class ListPrint
    {
        public static void printList(List<DataBaseBook> books) {
            int i = 0;
            foreach (DataBaseBook book in books)
            {
                Console.Write($"{i}: ");
                book.printDetailsOfBook();
                Console.WriteLine();
                i++;
            }
        }
    }
}
