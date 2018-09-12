using System;
using System.Linq;
using System.Threading;
using System.Data.SqlClient;

namespace Logger
{
    enum Type:byte
    {
        Login,
        Logout,
        Error
        
    }
    class Program
    {
        static  Model model;
        static void Main(string[] args)
        {
            if(args.Length>=0)
            {
                Type arg=Type.Logout;
                try
                {
                    arg = (Type)Enum.Parse(typeof(Type), args[0], true);
                }
                catch (Exception){}
                model = new Model(arg,"LoggerDB");
                model.Start();
                Console.SetCursorPosition(0, 1);
                Console.WriteLine("Await Please");
                model.Dispose();
                Console.ReadKey();
            }
            
        }
    }
}
