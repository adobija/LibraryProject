using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.exceptions;
using LibraryProject.libraries;

namespace LibraryProject.abstracts
{
    public abstract class BookAbstract
    {
        public string Title {  get; set; } 
        public string Author { get; set; } 
        public long ISBN { get; set; }
        public bool IsAvailable { get; set; }
        public string Category { get; set; }

        public int borrowCount { get; set; } = 0;

        protected BookAbstract(string title, string author, long isbn, string Category)
        {
            this.Title = title;
            this.Author = author;

            bool isValid = true;
            do
            {
                try
                {
                    isValid = !ISBNValidator.isValid(isbn);
                }
                catch (InvalidISBN e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                
            } while (isValid);
            

            this.IsAvailable = true;
            this.Category = Category;
        }
        public BookAbstract(string title, string author, long iSBN, bool isAvailable, string category)
        {
            Title = title;
            Author = author;
            ISBN = iSBN;
            IsAvailable = isAvailable;
            Category = category;
        }

        public virtual void printDetailsOfBook() {


            string refactoredISBN = refactorISBN();

            String output = $"Book {Title} written by {Author} ({refactoredISBN})";

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

        public void plusBorrowCount()
        {
            this.borrowCount++;
        }
    }
}
