using Npgsql;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace YourDoctor
{
     class Connection
    {
        //public static NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Database=YourDoctors;" +
        //    "User Id=postgres;Password=Devilmaycry135790;");

        static string host = "rc1b-vgck3dsn3lxaqtk1.mdb.yandexcloud.net";
        static string port = "6432";
        static string db = "YourDoctor";
        static string username = "MostWanted";
        static string password = "#(rspeedneedfo'_";
        static string connString = $"Host={host};Port={port};Database={db};Username={username};Password={password};Ssl Mode=Require; Trust Server Certificate=true;";

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

    }
}

