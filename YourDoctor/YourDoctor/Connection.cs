using Npgsql;
using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace YourDoctor
{
     class Connection
    {

        static string connString = $"Host={Properties.Resources.host};Port={Properties.Resources.port};Database={Properties.Resources.db};Username={Properties.Resources.dark};Password={Properties.Resources.souls};SSL Mode=VerifyFull;" ;
        
        public static NpgsqlConnection conn = new NpgsqlConnection(connString);

    public static DataSet ds = new DataSet();

        public bool OpenConnection()
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (NpgsqlException e)
            {
                MessageBox.Show($"Ошибка:Не удалось подключиться к серверу. \nПроверьте работу сервера или перезапустите его." +
                    $" \nТак же проверьте наличие подключения к сети","Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static void Table_Fill(string name, string sql)
        {
            if (ds.Tables[name] != null)
                ds.Tables[name].Clear();
            using (var adapter = new NpgsqlDataAdapter(sql, conn))
            {
                
                adapter.Fill(ds,name);
                conn.Close();
            }
        }

        public void close_conn()
        {
            conn.Close();
        }
        public NpgsqlConnection get_connection()
        {
            return conn;
        }

        public static bool Modification_Execute(string sql)
        {
            
            using (var com = new NpgsqlCommand(sql, conn))
            {
                conn.Open();
                try
                {
                    com.ExecuteNonQuery();
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show($"Ошибка:{e}");
                    conn.Close();
                    return false;
                }
                conn.Close();
                return true;
            }
        }

    }
}

