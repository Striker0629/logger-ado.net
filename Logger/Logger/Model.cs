using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Logger
{
    class Model
    {
        //private EventType eventType;
        private ILogger logger;
        public Model(EventType type)
        {
        
          switch(type)
            {
                case EventType.Login:
                    logger = new Login(Model.GetConnectionString());
                    break;
                case EventType.Logout:
                    logger = new Logout(Model.GetConnectionString());
                    break;
                case EventType.NoArgument:
                    logger = new NoArgument(Model.GetConnectionString());
                    break;
                default:
                    throw new IncorrectArgs("Incorrect argument");
            }
                    

        }
        public void Start()
        {

            logger.Log();
        }
        

       public static String GetConnectionString(string initial="LoggerDB")
        {
            //DESKTOP - PC73D7E\SQLEXPRESS
            return String.Format(@"server=localhost\SQLEXPRESS;database={0};integrated Security=SSPI", initial);

        }


    }
}
