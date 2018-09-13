using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Logger
{
    class Model : IDisposable
    {
        private SqlConnection connection;
        private DataTable table;
        private string UserName;
        private string PcName;
        private EventType eventType;
        private ILogger logger;
        public Model(EventType evt, string init = "master")
        {
            eventType = evt;
            connection = new SqlConnection(GetConnectionString(init));
            table = new DataTable();
            UserName = Environment.UserName;
            PcName = Environment.MachineName;

        }
        public void Start()
        {
            switch (eventType)
            {
                case EventType.Login:
                    logger = new Login(GetConnectionString("master"));
                    
                    logger.Log();
                    break;
                case EventType.Logout:
                    //InsertDataLogout();
                    break;
                default:
                    throw new ArgumentException("Undefined Argument");

            }
        }
        private void InsertDataLogin()
        {
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO dbo.Log(PcName,UserName,Type,Time) VALUES(@p1,@p2,@p3,@p4)";
            command.Parameters.Add("@p1", SqlDbType.NVarChar);
            command.Parameters.Add("@p2", SqlDbType.NVarChar);
            command.Parameters.Add("@p3", SqlDbType.TinyInt);
            command.Parameters.Add("@p4", SqlDbType.DateTime);
            command.Parameters["@p1"].Value = PcName;
            command.Parameters["@p2"].Value = UserName;
            command.Parameters["@p3"].Value = eventType;
            command.Parameters["@p4"].Value = DateTime.Now;
            command.ExecuteNonQuery();
            command.Dispose();
        }
        private void InsertDataLogout()
        {
            var lastLogin = ReadLastLogin();
           // Console.WriteLine(lastLogin);
        }
        private DateTime ReadLastLogin()
        {
            SqlParameter returnValue;
            //returnValue.SqlDbType = SqlDbType.DateTime;
            connection.Open();
            using (var command = connection.CreateCommand())
            {

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.GetLastLoginProc";
                command.Parameters.Clear();
                //new SqlParameter(); localhost\SQLEXPRESS
                command.Parameters.AddWithValue("@pcname",PcName).SqlDbType=SqlDbType.NVarChar;
                command.Parameters.AddWithValue("@user_name",UserName).SqlDbType= SqlDbType.NVarChar;
                command.Parameters.AddWithValue("@type",EventType.Login).SqlDbType=SqlDbType.TinyInt;
                returnValue = new SqlParameter("@return", SqlDbType.DateTime);
                
                returnValue.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnValue);
                command.ExecuteNonQuery();
                connection.Close();
                command.Dispose();
              
            }
            Console.WriteLine(returnValue.Value);
            return DateTime.Now;
        }

        static String GetConnectionString(string initial)
        {
            //DESKTOP - PC73D7E\SQLEXPRESS
            return String.Format(@"server=localhost\SQLEXPRESS;database=LoggerDB;integrated Security=SSPI", initial);

        }
        //public Action<void> Log => logger.Log;

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
