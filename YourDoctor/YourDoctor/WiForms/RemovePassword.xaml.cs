using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace YourDoctor.WiForms
{
    /// <summary>
    /// Логика взаимодействия для RemovePassword.xaml
    /// </summary>
    public partial class RemovePassword : Window
    {
        public RemovePassword()
        {
            InitializeComponent();
            Connection.Table_Fill("RDR", "SELECT\r\n  \"authorization\".id,\r\n  \"authorization\".login,\r\n  COALESCE(doctor.phone, nurse.phone) AS phone\r\n" +
                "FROM\r\n  \"authorization\"\r\nLEFT JOIN\r\n  doctor ON" +
                " \"authorization\".doctor_id = doctor.id\r\nLEFT JOIN\r\n  nurse ON \"authorization\".nurse_id = nurse.id where \"authorization\".id >1;");
        }
        Authorization_window authorization_Window = new Authorization_window();
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {

            authorization_Window.Show();
            this.Close();
        }
        string id = "";
        private void btn_reload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string formattedPhoneNumber = Generate.FormatPhoneNumber(txtPhone.Text);

                txtPhone.Text = formattedPhoneNumber;
            }
            catch (ArgumentException ex)
            {
                // Выводим сообщение об ошибке в формате номера телефона
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
                return;
            }
            // Поиск логина и телефона в таблице
            DataRow[] rows = Connection.ds.Tables["RDR"].Select($"login = '{txtLogin.Text}'");
            if (rows.Length > 0)
            {
                // Проверка по номеру телефона
                bool isPhoneMatch = false;
                foreach (DataRow row in rows)
                {
                    if (row["phone"].ToString() == txtPhone.Text)
                    {
                        isPhoneMatch = true;
                        id = row["id"].ToString(); 
                        break;
                    }
                }

                if (isPhoneMatch)
                {
                    string password = Generate.Password(Generate.Transliterate(txtLogin.Text.Split(' ')[0]));
                    string sql = $"UPDATE \"authorization\" SET password='{Generate.transHash(password)}' where id={id}";
                    if (!Connection.Modification_Execute(sql))
                        return;
                    Connection.Table_Fill("Авторизация", "select *from \"authorization\" order by id asc;");
                    MessageBox.Show($"Сброшен пароль для логина:{txtLogin.Text}.\n Новый пароль для данного пользователя:{password}", "Сброс пароля",MessageBoxButton.OK,MessageBoxImage.Information);
                    authorization_Window.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Номер указан неверно.");
                }
            }
            else
            {
                MessageBox.Show("Логин и номер телефона не найдены.");
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
