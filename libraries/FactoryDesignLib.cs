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
            else if (type.Equals("ADVENTURE"))
            {
                return new AdventureBook();
            }
            else if (type.Equals("BIOGRAPHY"))
            {
                return new BiographyBook();
            }
            else if (type.Equals("DYSTOPIAN"))
            {
                return new DystopianBook();
            }
            else if (type.Equals("HISTORY"))
            {
                return new HistoryBook();
            }
            else if (type.Equals("HORROR"))
            {
                return new HorrorBook();
            }
            else if (type.Equals("PSYCHOLOGY"))
            {
                return new PsychologyBook();
            }
            else if (type.Equals("SCIENCE"))
            {
                return new ScienceBook();
            }
            else
            {
                throw new NotImplementedException($"{type} category does not exist!");
            }

        }
    }
}
