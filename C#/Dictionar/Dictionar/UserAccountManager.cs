using System;
using System.Collections.Generic;
using System.IO;

namespace Dictionar
{
    public class UserAccountManager
    {
        private static Dictionary<string, string> userAccounts;

        static UserAccountManager()
        {
            LoadUserAccounts();
        }

        private static void LoadUserAccounts()
        {

            userAccounts = new Dictionary<string, string>();

            try
            {
                string[] lines = File.ReadAllLines("conturi.txt");
                foreach (string line in lines)
                {
                    string[] parts = line.Split(' ');
                    if (parts.Length == 2)
                    {
                        userAccounts.Add(parts[0], parts[1]);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Fisierul de conturi de utilizatori nu a fost gasit.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("A aparut o eroare la incarcarea conturilor de utilizatori: " + ex.Message);
            }
        }

        public bool ValidateUser(string username, string password)
        {
            LoadUserAccounts();

            if (userAccounts.ContainsKey(username))
            {
                string storedPassword = userAccounts[username];

                return storedPassword == password;
            }
            return false;
        }
    }
}

