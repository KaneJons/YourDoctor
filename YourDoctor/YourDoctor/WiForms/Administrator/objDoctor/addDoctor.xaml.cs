using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YourDoctor.WiForms.Administrator.objDoctor
{
    /// <summary>
    /// Логика взаимодействия для addDoctor.xaml
    /// </summary>
    public partial class addDoctor : Page
    {
        public addDoctor()
        {
            InitializeComponent();
            Dictionary<char, string> gender = new Dictionary<char, string>()
            {
                {'М',"Мужской" },
                {'Ж',"Женский" },
            };
            comboBox1.ItemsSource = gender;
        }

        private void btn_ReturnPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new pageDoctor());
        }


        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            var doctor = Connection.ds.Tables["Доктор"];
            var authoriz = Connection.ds.Tables["Авторизация"];
            var pole = comboBox1.SelectedValue.ToString();
            var id = Convert.ToInt32(doctor.Rows[doctor.Rows.Count - 1]["id"]) + 1;
            var ids = Convert.ToInt32(authoriz.Rows[authoriz.Rows.Count - 1]["id"]) + 1;
            string login = Generate.Transliterate($"{txtFamily.Text} {txtImy.Text.Substring(0, 1)}.{txtOtchestvo.Text.Substring(0, 1)}.");
            string password = Generate.Password(Generate.Transliterate(txtFamily.Text));
            string messageBoxtxt = "";
            try
            {
                string formattedPhoneNumber = Generate.FormatPhoneNumber(txtNumberPhone.Text);

                txtNumberPhone.Text = formattedPhoneNumber;
            }
            catch (ArgumentException ex)
            {
                // Выводим сообщение об ошибке в формате номера телефона
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
                return;
            }

            try
            {
                //sql = "INSERT INTO nurse " +
                //    $"VALUES ({id}, '{txtFamily.Text}', '{txtImy.Text}', '{txtOtchestvo.Text}', '{txtAddress.Text}', '{txtNumberPhone.Text}', '{datePicker.Text}', '{comboBox1.SelectedValue.ToString()}');";
                sql = "BEGIN TRANSACTION; " +
      $"INSERT INTO doctor VALUES ({id}, '{txtFamily.Text}', '{txtImy.Text}', '{txtOtchestvo.Text}', '{txtAddress.Text}', '{txtNumberPhone.Text}', '{txtSpecialist.Text}','{datePicker.Text}', '{pole}'); " +
      $"INSERT INTO \"authorization\" VALUES ({ids}, '{login}', " +
      $"'{Generate.transHash(password)}','Врач',{id},NULL); " +
      "COMMIT;";
                if (!Connection.Modification_Execute(sql))
                    return;
                switch(pole)
                    {
                    case "М":
                        messageBoxtxt = "ему";
                        break;
                    case "Ж":
                        messageBoxtxt = "ей";
                        break;
                    }
                MessageBox.Show($"Добавлен новый врач!!! \n Передайте {messageBoxtxt} следующий логин и пароль:\n" +
                    $"Логин: {login}\n Пароль: {password}", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            doctor.Rows.Add(new object[] { id, txtFamily.Text,txtImy.Text,txtOtchestvo.Text, txtAddress.Text, txtNumberPhone.Text, txtSpecialist.Text,datePicker.Text,pole});



            this.NavigationService.Navigate(new pageDoctor());
            //var slovo = Generate.Transliterate(txtFamily.Text);
            //txtImy.Text = slovo;   
            //txtOtchestvo.Text = Generate.Password(slovo);
        }




        private const string defaultText = "Пример: +7 987 654-32-10";

        private void txtNumberPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            txtNumberPhone.Text = txtNumberPhone.Text == defaultText ? string.Empty : txtNumberPhone.Text;

        }

        private void txtNumberPhone_LostFocus(object sender, RoutedEventArgs e)
        {
            txtNumberPhone.Text = txtNumberPhone.Text == string.Empty ? defaultText : txtNumberPhone.Text;
        }
    }
}
