using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YourDoctor.WiForms.Administrator;

namespace YourDoctor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Authorization_window : Window
    {
        public Authorization_window()
        {
            InitializeComponent();
            Connection.Table_Fill("Авторизация", "select *from \"authorization\";");
        }

        string username, password, sql1;
        Connection connection = new Connection();
        NpgsqlCommand command;
        bool flag = false;

        private void Button_Click_login(object sender, RoutedEventArgs e)
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
                if (connection.OpenConnection() == true)
                {
                    try
                    {
                        var loginDict = Connection.ds.Tables["Авторизация"].AsEnumerable()
                            .ToDictionary(row => row.Field<string>("login"), row => row.Field<string>("password"));

                        if (loginDict.ContainsKey(textbox1.Text))
                        {

                           if(testPassw(loginDict[textbox1.Text], password))
                            {
                                Administrator administrator= new Administrator();
                                administrator.Show();
                                this.Close();
                            }

                           
                        }
                        else
                        {
                            MessageBox.Show("Неверно введен Логин/Пароль, повторите попытку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    catch (NpgsqlException x)
                    {
                        MessageBox.Show($"{x}");
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
                MessageBox.Show("Неверно введен Логин/Пароль, повторите попытку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }


        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
    }
}
