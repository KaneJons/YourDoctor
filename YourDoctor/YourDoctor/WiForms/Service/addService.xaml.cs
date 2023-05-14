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
using YourDoctor.WiForms.Patient;

namespace YourDoctor.WiForms.Service
{
    /// <summary>
    /// Логика взаимодействия для addService.xaml
    /// </summary>
    public partial class addService : Page
    {
        public addService()
        {
            InitializeComponent();
        }
        private void btn_ReturnPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new pageService());
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            string text = txtCoast.Text;

            // Проверяем наличие некорректных символов (кроме цифр, '.' и ',')
            if (text.Any(c => !char.IsDigit(c) && c != '.' && c != ','))
            {
                MessageBox.Show("Ошибка! Введите число в правильном формате.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            // Проверяем наличие нескольких точек и запятых
            int dotCount = text.Count(c => c == '.');
            int commaCount = text.Count(c => c == ',');
            if (dotCount > 1 || commaCount > 1 || (dotCount == 1 && commaCount == 1))
            {
                MessageBox.Show("Ошибка! Введите число в правильном формате.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            // Если есть ',', заменяем его на '.' и устанавливаем новое значение в TextBox
            if (text.Contains(","))
            {
                text = text.Replace(",", ".");
                txtCoast.Text = text;
            }

            // Проверяем, стоит ли после '.' больше двух цифр
            int dotIndex = text.IndexOf(".");
            if (dotIndex != -1 && dotIndex < text.Length - 1)
            {
                string afterDot = text.Substring(dotIndex + 1);
                if (afterDot.Length > 2)
                {
                    MessageBox.Show("Ошибка! Введите число в правильном формате.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }
            }
            int dotIndexs = text.IndexOf(".");
            if (dotIndexs != -1 && dotIndexs == text.Length - 1)
            {
                text = text.Remove(dotIndexs);
                txtCoast.Text = text;
            }

            var service = Connection.ds.Tables["Услуга"];
            var id = Convert.ToInt32(service.Rows[service.Rows.Count - 1]["id"]) + 1;

            try
            {
                sql = $"INSERT INTO service VALUES ({id}, '{txtName.Text}', '{txtDescription.Text}', {txtCoast.Text}); ";
                if (!Connection.Modification_Execute(sql))
                    return;
                MessageBox.Show($"Успешно добавлена новая услуга", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            service.Rows.Add(new object[] { id, txtName.Text, txtDescription.Text, txtCoast.Text.Replace(".", ",") });



            this.NavigationService.Navigate(new pageService());
            

        }
    }
}
