using System;
using System.Linq;
using System.Data.SqlClient;

namespace Logger
{
    class Program
    {
        static string login;
        static string logout;
        private Model model;
        public Program()
        {
            model = new Model();
        }
        static Program()
        {
            login = "login";
            logout = "logout";
        }
        static void Main(string[] args)
        {
            Program program = new Program();
            if (args.Contains(login))
            {
                Console.WriteLine("Login");
                program.model.InsertDataLogin();
            }
            else if (args.Contains(logout))
            {
                Console.WriteLine("Logout");
            }
            else
                throw new ArgumentException("Неверный параметр коммандной строки");
            Console.ReadKey();
        }




    }
}
