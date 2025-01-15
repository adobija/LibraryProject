using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.classes;

namespace LibraryProject.libraries
{
    internal class RefactorUserDb
    {
        public static List<User> FetchUsers()
        {
            string pathToTxt = "../../../libraries/UserDB.txt";
            List<User> users = new List<User>();
            try
            {
                using (StreamReader usersDB = new StreamReader(pathToTxt))
                {
                    while (usersDB.Peek() >= 0)
                    {
                        string[] line = usersDB.ReadLine().Split(",");
                        User user = new User(line[0], line[1], int.Parse(line[2]));
                        users.Add(user);
                    }

                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("UserController fail: UserDB.txt file not found");
            }

            return users;
        }
    }
}
