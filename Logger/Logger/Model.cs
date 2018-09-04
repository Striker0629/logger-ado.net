using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Logger
{
    class Model
    {
        private SqlConnection connection;
        private DataTable table;
        public Model(string init="master",bool integr=true)
        {
            connection = new SqlConnection(GetConnectionString(init, integr));
            table = new DataTable();

        }

        public void InsertData()
        {

        }

        public void ReadLastLogin()
        {
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT date TOP(1) FROM dbo.Log WHERE pc_name=@p1";
            
            SqlParameter param = command.CreateParameter();
            param.SqlDbType = SqlDbType.NVarChar;
            param.ParameterName = "@p1";
            param.Value = Environment.MachineName;
            SqlDataReader reader = command.ExecuteReader();
            command.ExecuteScalar();
        }

        static String GetConnectionString(string initial,bool integrated)
        {
            String  connect=System.Configuration.ConfigurationManager.ConnectionStrings["Connect"].ConnectionString;
            return String.Format("Data Source={0};Initial Catalog={1};Integrated Security={2};", connect, initial, integrated);
        }


    }
}
