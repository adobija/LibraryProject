using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.abstracts;

namespace LibraryProject.classes
{
    public class DataBaseBook : BookAbstract
    {
        public DataBaseBook(string title, string author, long isbn, string Category, bool IsAvailable) : base(title, author, isbn, IsAvailable, Category )
        {

        }
    }
}
