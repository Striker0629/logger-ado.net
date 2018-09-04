using System;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Logger
{
    class Model
    {
        private SqlConnection connection;
        public Model(string init="master",bool integr=true)
        {
            connection = new SqlConnection(GetConnectionString(init, integr));

        }

        public void InsertData()
        {

        }

        public void ReadData()
        {

        }

        static String GetConnectionString(string initial,bool integrated)
        {
            String  connect=System.Configuration.ConfigurationManager.ConnectionStrings["Connect"].ConnectionString;
            return String.Format("Data Source={0};Initial Catalog={1};Integrated Security={2};", connect, initial, integrated);
        }


    }
}
