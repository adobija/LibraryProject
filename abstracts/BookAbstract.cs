using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.exceptions;

namespace LibraryProject.abstracts
{
    internal abstract class BookAbstract
    {
        private string Title;
        private string Author;
        private long ISBN;

        protected BookAbstract(string title, string author, long isbn)
        {
            this.Title = title;
            this.Author = author;
            if (isbn < 1000000000000 || isbn > 9999999999999)
            {
                throw new InvalidISBN();
            }
            else {
                this.ISBN = isbn;
            }
        }

        public virtual void printDetailsOfBook() {


            string refactoredISBN = refactorISBN();

            String output = $"The book called {Title} written by {Author} ({refactoredISBN})";

            Console.Write(output);
        }

        private string refactorISBN() { 
            long isbn = getISBN();
            
            string refactoredISBN = isbn.ToString();

            string fifthCluster = refactoredISBN.Substring(0, 3);
            string fourthCluster = refactoredISBN.Substring(3, 1);
            string thirdCluster = refactoredISBN.Substring(4, 2);
            string secondCluster = refactoredISBN.Substring(6, 6);
            string firstCluster = refactoredISBN.Substring(12, 1);
 
            string output = $"{fifthCluster}-{fourthCluster}-{thirdCluster}-{secondCluster}-{firstCluster}";
            return output;
        }
        private long getISBN()
        {
            return this.ISBN;
        }
    }
}
