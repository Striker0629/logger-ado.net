using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    class NoArgument : ILogger
    {
        private String connectionString;
        public NoArgument(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void Log()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command=connection.CreateCommand())
                {
                    if (connection.State != System.Data.ConnectionState.Open) connection.Open();
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT PcName,UserName,Time,Type,TimeInSeconds FROM dbo.Log as l LEFT JOIN dbo.WorkedTime as w ON  l.Id=w.LogId " +
                        " WHERE l.UserName=@username AND l.PcName=@pcname" +" "+
                        "AND l.Time>=DATEDIFF(day,1,GETDATE())";
                    command.Parameters.AddWithValue("@username", Environment.UserName).SqlDbType=System.Data.SqlDbType.NVarChar;
                    command.Parameters.AddWithValue("@pcname", Environment.MachineName).SqlDbType = System.Data.SqlDbType.NVarChar;
                  var reader=command.ExecuteReader();
                    while(reader.Read())
                    {
                        //for(int i=0;i<reader.FieldCount;++i)
                            Console.WriteLine(string.Format("pcname {0} username {1} time{2} " +
                                "type {3} workedtime {4} ",reader["PcName"],reader["UserName"],reader["Time"],
                                ToTypeString(reader["Type"]),reader["TimeInSeconds"]));
                    }
                    connection.Close();
                }
            }
        }

        static private object ToTypeString(object v)
        {
            //int value = (int)v;
            return Enum.GetName(typeof(EventType), v);
        }

        //static string EventTypeToString(this EventType t, int value)
        //{
        //    EventType args = Enum.F
        //}
    }
}
