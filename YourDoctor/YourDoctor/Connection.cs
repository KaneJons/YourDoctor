using Npgsql;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace YourDoctor
{
     class Connection
    {
        public static NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Database=YourDoctors;" +
            "User Id=postgres;Password=Devilmaycry135790;");
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
                MessageBox.Show($"Ошибка:{e}");
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

        public static void FillGrid(string sql, DataGrid dataGrid)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection("Server=localhost; Database=YourDoctor;User " +
                "Id=postgres;Password=Devilmaycry135790;"))
            {
                connection.Open();
                using (NpgsqlCommand cmdSel = new NpgsqlCommand(sql, connection))
                {
                    DataTable dt = new DataTable();
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmdSel);
                    da.Fill(dt);

                    dataGrid.ItemsSource = dt.DefaultView;
                }
                connection.Close();
            }
        }
    }
}

