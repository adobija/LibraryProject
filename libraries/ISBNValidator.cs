using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.classes;
using LibraryProject.exceptions;

namespace LibraryProject.libraries
{
    internal class ISBNValidator
    {

        public static bool isValid(long isbn) {
            if (isbn < 1000000000000 || isbn > 9999999999999)
            {
                throw new InvalidISBN();
            }
            else
            {
                return true;
            }
        }

        public static async Task<bool> isInDatabaseAsync(long isbn) {
            List<DataBaseBook> dataBaseBooks = await LibrarySystem.LoadBooksAsync();
            foreach (DataBaseBook book in dataBaseBooks) {
                if (book.ISBN == isbn) { 
                throw new ISBNAlreadyExistException();
                }
            }
            return false;
        }
    }
}
