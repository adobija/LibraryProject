using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LibraryProject.classes;

namespace LibraryProject.interfaces.implementations
{
    internal class UserRegisterImpl : UserRegister
    {
        private string UserDB = @"../../../libraries/UserDB.json";
        public async void RegisterUser(string userName, string password)
        {
            

            User user = new User(userName, password);

            try
            {

                using (StreamWriter file = new StreamWriter(UserDB, true))
                {
                    string json = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true }); // Serializacja do JSON
                    await file.WriteAsync(json);
                }
                Success();
            }
            catch (FileNotFoundException e)
            { 
            Console.WriteLine("UserRegisterImpl fail: file UserDB.json not found");
            }
        }

        public void Success()
        {
            Console.WriteLine("Successfully registered!");
        }

    }
}
