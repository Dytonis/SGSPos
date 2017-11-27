using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGSPos.Service
{
    public struct User
    {
        public string Username;
        public string Pulid;
        public PermissionGroup Permissions;
    }

    public static class Users
    {
        public static User UserLogin { get; set; }

        private static List<User> UserDatabase = new List<User>();

        public static bool RegisterUser(string u, string p, out string result)
        {
            if (UserDatabase.Count <= 0)
                GetJson();

            if(UserDatabase.Any(x => x.Username == u))
            {
                result = "Username already taken.";
                return false;
            }
            else
            {
                User user = new User()
                {
                    Username = u,
                };

                WriteJson(user);
                result = "User created.";
                return true;
            }
        }

        private static void WriteJson(User u)
        {
            if (UserDatabase.Count <= 0)
                GetJson();

            UserDatabase.Add(u);
            User[] array = UserDatabase.ToArray();

            string write = JsonConvert.SerializeObject(array);

            File.WriteAllText(@"users.json", write);
        }

        private static void GetJson()
        {
            string json = "";

            if (File.Exists(@"users.json"))
            {
                json = File.ReadAllText(@"config.json");
            }
            else
            {
                User[] us = new User[]
                {
                    
                };

                string write = JsonConvert.SerializeObject(us);

                File.WriteAllText(@"users.json", write);

                MessageBox.Show("There was no user file. It has been made at the install location.", "Info", MessageBoxButtons.OK);
            }

            try
            {
                User[] r = JsonConvert.DeserializeObject<User[]>(json);

                if (json != null && r != null)
                {
                    UserDatabase = r.ToList();
                }
            }
            catch
            {
                User[] us = new User[]
                {

                };

                string write = JsonConvert.SerializeObject(us);

                File.WriteAllText(@"users.json", write);

                MessageBox.Show("The user file was not in the right format and could not be read. It has been re-made at the install location.", "Info", MessageBoxButtons.OK);
            }
        }
    }
}
