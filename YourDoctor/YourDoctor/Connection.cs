﻿using Npgsql;
using System.Data;
using System.Windows;

namespace YourDoctor
{
     class Connection
    {

        static string connString = $"Host={Properties.Resources.host};Port={Properties.Resources.port};Database={Properties.Resources.db};" +
            $"Username={Properties.Resources.dark};Password={Properties.Resources.souls};SSL Mode=VerifyFull;" ;
        
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
                    if (e.Message.Contains("duplicate key value")) // Проверка наличия ключевой фразы в сообщении об ошибке
                    {
                        MessageBox.Show("Ошибка: Создание дубликата существующего уже объекта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (e.Message.Contains("column") && e.Message.Contains("does not exist")) // Дополнительная проверка на другую возможную фразу в сообщении об ошибке
                    {
                        MessageBox.Show("Ошибка: Введено не корректное значение, пожалуйста исправьте это.\n" +
                            "Пример ошибки: Введен символ '@', а ожидалась цифра", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка: " + e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    conn.Close();
                    return false;
                }
                conn.Close();
                return true;
            }
        }

    }
}

