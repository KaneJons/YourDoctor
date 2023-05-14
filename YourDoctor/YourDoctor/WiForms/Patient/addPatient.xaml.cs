using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using YourDoctor.WiForms.Administrator.objDoctor;

namespace YourDoctor.WiForms.Patient
{
    /// <summary>
    /// Логика взаимодействия для addPatient.xaml
    /// </summary>
    public partial class addPatient : Page
    {
        public addPatient()
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
            this.NavigationService.Navigate(new pagePatient());
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            var patient = Connection.ds.Tables["Пациент"];
            var pole = comboBox1.SelectedValue.ToString();
            var id = Convert.ToInt32(patient.Rows[patient.Rows.Count - 1]["id"]) + 1;
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
                sql =$"INSERT INTO patient VALUES ({id}, '{txtFamily.Text}', '{txtImy.Text}', '{txtOtchestvo.Text}', '{txtAddress.Text}', '{txtNumberPhone.Text}','{datePicker.Text}', '{pole}'); ";
                if (!Connection.Modification_Execute(sql))
                    return;
                MessageBox.Show($"Успешно добавлен новый пациент", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            patient.Rows.Add(new object[] { id, txtFamily.Text, txtImy.Text, txtOtchestvo.Text, txtAddress.Text, txtNumberPhone.Text, datePicker.Text, pole });



            this.NavigationService.Navigate(new pagePatient());
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
