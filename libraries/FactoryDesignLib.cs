using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.abstracts;
using LibraryProject.classes;
using LibraryProject.classes.bookClasses;

namespace LibraryProject.libraries
{
    internal class FactoryDesignLib
    {
        public static BookAbstract getBook(String type)
        {
            if (type.Equals("FANTASY"))
            {
                return new FantasyBook();
            }
            else if (type.Equals("MYSTERY"))
            {
                return new MysteryBook();
            }
            else if (type.Equals("ROMANCE"))
            {
                return new RomanceNovelBook();
            }
            else
            {
                return null;
            }

        }
    }
}
