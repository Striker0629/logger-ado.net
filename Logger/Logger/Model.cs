﻿using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Logger
{
    class Model : IDisposable
    {
        private SqlConnection connection;
        private DataTable table;
        string UserName;
        string PcName;
        private Type eventType;
        public Model(Type evt, string init = "master", bool integr = true)
        {
            eventType = evt;
            connection = new SqlConnection(GetConnectionString(init,integr));
         //   connection.ConnectionString = GetConnectionString(init, integr);
            //  connection = new SqlConnection(GetConnectionString(init, integr));
          
            table = new DataTable();
            UserName = Environment.UserName;
            PcName = Environment.MachineName;

        }
        public void Start()
        {
            switch (eventType)
            {
                case Type.Login:
                    try
                    {
               
                        InsertDataLogin();
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                   
                    break;
                case Type.Logout:
                    InsertDataLogout();
                    break;
                default:
                    break;
            }
        }
        private  void InsertDataLogin()
        {
            connection.Open();
            SqlCommand command = new SqlCommand();
            
            command.CommandText = "INSERT INTO dbo.Log(PcName,UserName,Type,Time) VALUES(@p1,@p2,@p3,@p4)";
            var p1 = command.CreateParameter();
            p1.ParameterName = "@p1";
            p1.SqlDbType = SqlDbType.NVarChar;
            p1.SqlValue = PcName;
            var p2 = command.CreateParameter();
            p2.ParameterName = "@p2";
            p2.SqlValue = UserName;
            p2.SqlDbType = SqlDbType.NVarChar;
            var p3 = command.CreateParameter();
            p3.ParameterName = "@p3";
            p3.SqlDbType = SqlDbType.TinyInt;
            p3.SqlValue = eventType;
            var p4 = command.CreateParameter();
            p4.ParameterName = "@p4";
            p4.SqlDbType = SqlDbType.DateTime;
            p4.SqlValue = DateTime.Now;
            command.ExecuteNonQuery();
            command.Dispose();

        }
        private void InsertDataLogout()
        {
            ReadLastLogin();
        }
        private void ReadLastLogin()
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

        static String GetConnectionString(string initial, bool integrated)
        {
            String connect = @"DESKTOP - PC73D7E\SQLEXPRESS";
            string ret;/*= @"Data Source=(localhostdb)\v13.0;Integrated Security=true;Initial Catalog=LoggerDB;";*/
            ret = @"server=DESKTOP-PC73D7E\SQLEXPRESS;database=LoggerDB;integrated Security=SSPI";
            return ret;
            
            //return String.Format("Data Source={0};Initial Catalog={1};Integrated Security={2};", connect, initial, integrated.ToString());
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
