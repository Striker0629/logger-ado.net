using System;

using System.Data;
using System.Data.SqlClient;


namespace Logger
{
    class Logout:ILogger
    {
        private String connectionString;
        public Logout(string Connection)
        {
            connectionString = Connection;
        }
        public void Log()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                ReadLastLogin(connection);
            }
        }

        private DateTime ReadLastLogin(SqlConnection connection)
        {
            SqlParameter returnValue;
            //returnValue.SqlDbType = SqlDbType.DateTime;
            using (var command = connection.CreateCommand())
            {
                if (connection.State != ConnectionState.Open) connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.GetLastLoginProc";
                command.Parameters.Clear();
                //new SqlParameter(); localhost\SQLEXPRESS
                command.Parameters.AddWithValue("@pcname", Environment.MachineName).SqlDbType = SqlDbType.NVarChar;
                command.Parameters.AddWithValue("@user_name", Environment.UserName).SqlDbType = SqlDbType.NVarChar;
                command.Parameters.AddWithValue("@type", EventType.Login).SqlDbType = SqlDbType.TinyInt;
                returnValue = new SqlParameter("@return", SqlDbType.DateTime);

                returnValue.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnValue);
                command.ExecuteNonQuery();
                connection.Close();
                command.Dispose();

            }
            Console.WriteLine(returnValue.Value);
            DateTime time = (DateTime)returnValue.Value;
            return time;
        }
    }
}
