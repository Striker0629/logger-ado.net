using System;
using System.Linq;
using System.Threading;
using System.Data.SqlClient;

namespace Logger
{
    enum EventType:byte
    {
        Login,
        Logout,
      
        
    }
    class Program
    {
        static  Model model;
        static void Main(string[] args)
        {
            if(args.Length>=0)
            {
                EventType arg=EventType.Logout;
                try
                {
                    arg = (EventType)Enum.Parse(typeof(EventType), args[0], true);
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
