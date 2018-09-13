using System;

using System.Data;
using System.Data.SqlClient;


namespace Logger
{
    class Logout : ILogger
    {
        private String connectionString;
        private DateTime current;
        public Logout(string Connection)
        {
            connectionString = Connection;
            current = DateTime.Now;
        }
        public void Log()
        {
            var connection = new SqlConnection(connectionString);

            var loginTime = ReadLastLogin(ref connection);
            var time =  current- loginTime.time;
            InsertData(ref connection, time.Seconds, loginTime.id);
            connection.Close();
            connection.Dispose();

        }

        private void InsertData(ref SqlConnection connection, int timeForInsert, int id)
        {
            using (var command = connection?.CreateCommand())
            {
                if (connection.State != ConnectionState.Open) connection.Open();
                command.CommandType = CommandType.Text;
                command.CommandText = string.Format("INSERT INTO dbo.Log(PcName,UserName,Type,Time) VALUES(@pcname,@username,@type,@time)");
                //command.Parameters.AddWithValue("@pcname", SqlDbType.NVarChar);
                //command.Parameters.Add("@username", SqlDbType.NVarChar);
                //command.Parameters.Add("@type", SqlDbType.TinyInt);
                //command.Parameters.Add("@time", SqlDbType.DateTime);
                command.Parameters.AddWithValue("@pcname", Environment.MachineName).SqlDbType = SqlDbType.NVarChar;
                command.Parameters.AddWithValue("@username", Environment.UserName).SqlDbType = SqlDbType.NVarChar;
                command.Parameters.AddWithValue("@type", EventType.Logout).SqlDbType = SqlDbType.TinyInt;
                command.Parameters.AddWithValue("@time", current).SqlDbType = SqlDbType.DateTime ;
                command.Parameters["@pcname"].SqlDbType = SqlDbType.NVarChar;
                command.Parameters["@username"].SqlDbType = SqlDbType.NVarChar;
                command.Parameters["@type"].SqlDbType = SqlDbType.Int;
                command.Parameters["@time"].SqlDbType = SqlDbType.DateTime;
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                command.CommandText = string.Format("INSERT INTO dbo.WorkedTime(LogId,TimeInSeconds) VALUES(@id,@time)");
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@time", timeForInsert);
                command.ExecuteNonQuery();
   
                connection.Close();
            }
        }

        private (DateTime time, int id) ReadLastLogin(ref SqlConnection connection)
        {
            SqlParameter returnValueTime;
            SqlParameter returnValueID;
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
                returnValueTime = new SqlParameter("@return", SqlDbType.DateTime);
                returnValueID = new SqlParameter("@returnid", SqlDbType.Int);
                returnValueTime.Direction = ParameterDirection.Output;
                returnValueID.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnValueTime);
                command.Parameters.Add(returnValueID);
                command.ExecuteNonQuery();
                connection.Close();
                command.Dispose();

            }
            //Console.WriteLine(returnValue.Value);
            DateTime time = (DateTime)returnValueTime.Value;
            Console.WriteLine(returnValueID.Value);
            int id = (int)returnValueID.Value;
            return (time, id);
        }
    }
}
