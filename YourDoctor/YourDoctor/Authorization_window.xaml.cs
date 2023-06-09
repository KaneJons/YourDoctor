﻿using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YourDoctor.WiForms;
using YourDoctor.WiForms.Administrator;
using YourDoctor.WiForms.Doctor;
using YourDoctor.WiForms.Nurse;

namespace YourDoctor
{

    public partial class Authorization_window : Window
    {
        public Authorization_window()
        {
            InitializeComponent();
            try
            {
                connection = new Connection();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось получить обратный ответ от сервера","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                Console.WriteLine($"Ошибка инициализации подключения: {ex}");
                // Дополнительная обработка ошибки
            }
            finally
            {
                connection?.close_conn();
            }
            Connection.Table_Fill("Авторизация", "select *from \"authorization\"  order by id asc;");
        }

        string password;
        Connection connection = null;


        private  void Button_Click_login(object sender, RoutedEventArgs e)
        {

            if (textbox2.Visibility == Visibility.Hidden && passwordBox.Visibility == Visibility.Visible)
            {
                password = passwordBox.Password;
            }
            else if (textbox2.Visibility == Visibility.Visible && passwordBox.Visibility == Visibility.Hidden)
            {
                password = textbox2.Text;
            }
            if (string.IsNullOrEmpty(textbox1.Text) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Логин/Пароль пуст", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (connection.OpenConnection()==true)
                {
                    try
                    {
                        var authorizationTable = Connection.ds.Tables["Авторизация"];
                        
                        var matchedRows = authorizationTable.AsEnumerable()
                            .Where(row => row.Field<string>("login") == textbox1.Text && testPassw(row.Field<string>("password"), password))
                            .ToList();

                        if (matchedRows.Count > 0)
                        {
                            DataRow matchedRow = matchedRows[0];
                            string role = matchedRow.Field<string>("role");

                            switch (role)
                            {
                                case "Администратор":
                                    Administrator administrator = new Administrator();
                                    try
                                    {
                                        administrator.Show();
                                    }
                                    finally
                                    {
                                        this.Close();
                                    }
                                    break;
                                case "Медсестра":
                                    string nurseId = matchedRow["nurse_id"].ToString();
                                    Connection.Table_Fill("Nurse", "select * from \"nurse\" where id = '" + nurseId + "' order by id asc;");
                                    DataRow[] nurseRows = Connection.ds.Tables["Nurse"].Select();
                                    if (nurseRows.Length > 0)
                                    {
                                        DataRow nurseRow = nurseRows[0];
                                        Nurse nurse = new Nurse($"{nurseRow["family"]} {nurseRow["imy"]}");
                                        nurse.Show();
                                    }
                                    Connection.ds.Tables["Nurse"].Clear();
                                    Connection.ds.Tables.Remove("Nurse");

                                    this.Close();
                                    break;
                                case "Врач":
                                    string doctorId = matchedRow["doctor_id"].ToString();
                                    Connection.Table_Fill("Doctor", "select * from \"doctor\" where id = '" + doctorId + "' order by id asc;");
                                    DataRow[] doctorRows = Connection.ds.Tables["Doctor"].Select();
                                    if (doctorRows.Length > 0)
                                    {
                                        DataRow doctorRow = doctorRows[0];
                                        Doctor doctor = new Doctor($"{doctorRow["family"]} {doctorRow["imy"]}");
                                        doctor.Show();
                                    }
                                    Connection.ds.Tables["Doctor"].Clear();
                                    Connection.ds.Tables.Remove("Doctor");

                                    this.Close();
                                    break;
                            }
                        }
                        else
                        {
                         MessageBox.Show("Неверно введен Логин/Пароль, повторите попытку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (NpgsqlException x)
                    {
                        MessageBox.Show("Перезапустите приложение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
            }

            textbox1.Text = "";
            textbox2.Text = "";
            passwordBox.Password = "";
            connection.close_conn();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked.Value)
            {
                textbox2.Text = passwordBox.Password;
                textbox2.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Hidden;
            }
            else
            {
                passwordBox.Password = textbox2.Text;
                textbox2.Visibility = Visibility.Hidden;
                passwordBox.Visibility = Visibility.Visible;
            }
        }


        public void RemovePassword(object sender, EventArgs e)
        {
            System.Windows.Controls.TextBox instance = (System.Windows.Controls.TextBox)sender;
            if (instance.Text == instance.Tag.ToString())
                instance.Text = "";
        }
        public void AddPassword(object sender, EventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(instance.Text))
                instance.Text = instance.Tag.ToString();
        }

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        public bool testPassw(string login,string password)
        {
            byte[] hashBytes = Convert.FromBase64String(login);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            bool passwordsMatch = true;
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    passwordsMatch = false;
                    break;
                }
            }

            if (!passwordsMatch)
            {
                //MessageBox.Show("Неверно введен Логин/Пароль, повторите попытку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void RemovePassword(object sender, RoutedEventArgs e)
        {
            if (textbox2.Visibility == Visibility.Hidden && passwordBox.Visibility == Visibility.Visible)
            {
                System.Windows.Controls.PasswordBox instance = (System.Windows.Controls.PasswordBox)sender;
                if (instance.Password == instance.Tag.ToString())
                    instance.Password = "";
            }
            else if (textbox2.Visibility == Visibility.Visible && passwordBox.Visibility == Visibility.Hidden)
            {
                System.Windows.Controls.TextBox instance = (System.Windows.Controls.TextBox)sender;
                if (instance.Text == instance.Tag.ToString())
                    instance.Text = "";
            }
        }

        private void AddPassword(object sender, RoutedEventArgs e)
        {
            if (textbox2.Visibility == Visibility.Hidden && passwordBox.Visibility == Visibility.Visible)
            {
                System.Windows.Controls.PasswordBox instance = (System.Windows.Controls.PasswordBox)sender;
                if (string.IsNullOrWhiteSpace(instance.Password))
                    instance.Password = instance.Tag.ToString();
            }
            else if (textbox2.Visibility == Visibility.Visible && passwordBox.Visibility == Visibility.Hidden)
            {
                System.Windows.Controls.TextBox instance = (System.Windows.Controls.TextBox)sender;
                if (string.IsNullOrWhiteSpace(instance.Text))
                    instance.Text = instance.Tag.ToString();
            }
        }

        private void RemoveText(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox instance = (System.Windows.Controls.TextBox)sender;
            if (instance.Text == instance.Tag.ToString())
                instance.Text = "";
        }

        private void AddText(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox instance = (System.Windows.Controls.TextBox)sender;
            if (string.IsNullOrWhiteSpace(instance.Text))
                instance.Text = instance.Tag.ToString();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RemovePassword removePassword = new RemovePassword();
            removePassword.Show();
            this.Close();
        }
    }
}
