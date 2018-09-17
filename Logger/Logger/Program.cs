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
        NoArgument
        
    }
    class Program
    {
        static  Model model;
        static void Main(string[] args)
        {
            
            try
            {
                if (args.Length > 0)
                {
                    EventType arg = (EventType)Enum.Parse(typeof(EventType), args[0], true);
                    model = new Model(arg);
                }
                else
                    model = new Model(EventType.NoArgument);
              
            }
            catch (IncorrectArgs except)
            {
                Console.WriteLine(except.Message);
            }
            model?.Start();
            Console.WriteLine("Await Please");
            Console.ReadKey();




        }
    }
}
