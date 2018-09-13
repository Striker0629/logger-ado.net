using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Logger
{
    class Model : IDisposable
    {
        //private EventType eventType;
        private ILogger logger;
        public Model(EventType type)
        {
        
          switch(type)
            {
                case EventType.Login:
                    logger = new Login(Model.GetConnectionString(""));
                    break;
                case EventType.Logout:
                    logger = new Logout(Model.GetConnectionString(""));
                    break;
                default:
                    throw new IncorrectArgs("Incorrect argument");
            }
                    

        }
        public void Start()
        {
            //switch (eventType)
            //{
            //    case EventType.Login:
            //        logger = new Login(GetConnectionString("master"));
                    
            //        logger.Log();
            //        break;
            //    case EventType.Logout:
            //        logger = new Logout(GetConnectionString("master"));
            //        logger.Log();
            //        //InsertDataLogout();
            //        break;
            //    default:
            //        throw new ArgumentException("Undefined Argument");

            //}
            logger.Log();
        }
        //private void InsertDataLogout()
        //{
        //    var lastLogin = ReadLastLogin();
        //   // Console.WriteLine(lastLogin);
        //}
        

       public static String GetConnectionString(string initial)
        {
            //DESKTOP - PC73D7E\SQLEXPRESS
            return String.Format(@"server=localhost\SQLEXPRESS;database=LoggerDB;integrated Security=SSPI", initial);

        }
        //public Action<void> Log => logger.Log;
        public ILogger Logger
        {
            get { return logger; }
            set
            {               
                    logger = value;
            }
        }
        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    connection.Close();
                    connection.Dispose();
                    table.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion


    }
}
