using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Logger
{
    class Model:IDisposable
    {
        private SqlConnection connection;
        private DataTable table;
        public Model(string init="master",bool integr=true)
        {
            connection = new SqlConnection(GetConnectionString(init, integr));
            table = new DataTable();

        }

        public void InsertDataLogin()
        {

        }
        public void InsertDataLogout()
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

        static String GetConnectionString(string initial,bool integrated)
        {
            String  connect=System.Configuration.ConfigurationManager.ConnectionStrings["Connect"].ConnectionString;
            return String.Format("Data Source={0};Initial Catalog={1};Integrated Security={2};", connect, initial, integrated);
        }

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    connection.Close();
                    connection.Dispose();
                   
                    table.Dispose();
                    // TODO: освободить управляемое состояние (управляемые объекты).
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~Model() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
