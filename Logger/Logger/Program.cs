using System;
using System.Linq;
using System.Threading;
using System.Data.SqlClient;

namespace Logger
{
    enum Type:byte
    {
        Login,
        Logout

    }
    class Program
    {
        static string login;
        static string logout;
        static  Model model;
    
        //public Program()
        //{
        //    model = new Model();
        //}
        static Program()
        {
            login = "login";
            logout = "logout";
        }
        static void Main(string[] args)
        {
            if (true)
            {
                Console.WriteLine("Login");
                model = new Model(Type.Login);

            }
            else if (args.Contains(logout))
            {
                Console.WriteLine("Logout");
                model = new Model(Type.Logout);
            }
            else
                throw new ArgumentException("Неверный параметр коммандной строки\n Для входа используйте login\n Для выхода logout\n");
            Thread tread = new Thread(model.Start);
            tread.Start();
            while(tread.IsAlive)
            {
                Console.SetCursorPosition(0, 1);
                Console.WriteLine("Await Please");
            }
            // model.Start();
            tread.Join();
            model.Dispose();
            Console.ReadKey();
        }




    }
}
