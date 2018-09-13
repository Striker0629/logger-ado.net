using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    class Login:ILogger
    {
        //private SqlConnection connection;
        private String connectionString;
        public Login(String conString)
        {
            connectionString = conString;
        }
        public void Log()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "INSERT INTO dbo.Log(PcName,UserName,Type,Time) VALUES(@p1,@p2,@p3,@p4)";
                    command.Parameters.Add("@p1", SqlDbType.NVarChar);
                    command.Parameters.Add("@p2", SqlDbType.NVarChar);
                    command.Parameters.Add("@p3", SqlDbType.TinyInt);
                    command.Parameters.Add("@p4", SqlDbType.DateTime);
                    command.Parameters["@p1"].Value = Environment.MachineName ;
                    command.Parameters["@p2"].Value = Environment.UserName;
                    command.Parameters["@p3"].Value = EventType.Login;
                    command.Parameters["@p4"].Value = DateTime.Now;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            //connection.Dispose();
        }

        #region Settings
        //public String Connection
        //{
        //    set;get;
        //}
        #endregion

    }

}
