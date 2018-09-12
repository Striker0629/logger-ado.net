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
        private Type eventType;

        public Model(Type evt, string init = "master")
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
                case Type.Login:
                    //try
                    //{

                    InsertDataLogin();
                    //}
                    //catch (InvalidOperationException ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //    Console.WriteLine(ex.StackTrace);
                    //}
                    break;
                case Type.Logout:
                    InsertDataLogout();
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
                
               //new SqlParameter()
                command.Parameters.AddWithValue("@pcname",PcName);
                command.Parameters.AddWithValue("@user_name",UserName);
                command.Parameters.AddWithValue("@type",eventType);
                 returnValue = command.Parameters.Add(new SqlParameter("@return", SqlDbType.DateTime));
                returnValue.Direction = ParameterDirection.Output;
                
                command.ExecuteNonQuery();
                //command.Dispose();
              
            }
            Console.WriteLine(returnValue.Value);
            return DateTime.Now;
        }

        static String GetConnectionString(string initial)
        {
            return String.Format(@"server=DESKTOP-PC73D7E\SQLEXPRESS;database=LoggerDB;integrated Security=SSPI", initial);

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
