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
    /// Логика взаимодействия для customerService.xaml
    /// </summary>
    public partial class customerService : Page
    {
        public int n;

        public customerService(int id)
        {
            InitializeComponent();
            n = id;
            if (Connection.ds.Tables["Услуга"].Rows.Count > n)
                FieldsForm_Fill();
        }

        private void FieldsForm_Fill()
        {
            using (var tab = Connection.ds.Tables["Услуга"])
            {
                txtName.Text = tab.Rows[n]["name"].ToString();
                txtCoast.Text = tab.Rows[n]["cost"].ToString();
                txtDescription.Text = tab.Rows[n]["description"].ToString();
            }
        }

        private void btn_ReturnPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new pageService());
        }

        string sql;
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
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
            try
            {
                string id = Connection.ds.Tables["Услуга"].Rows[n]["id"].ToString();
                sql = $"UPDATE service SET name='{txtName.Text}', description='{txtDescription.Text}', cost={txtCoast.Text} WHERE id={id};";

                if (!Connection.Modification_Execute(sql))
                    return;

                Connection.ds.Tables["Услуга"].Rows[n].ItemArray = new object[] { id, txtName.Text, txtDescription.Text, txtCoast.Text.Replace(".", ",") };
                MessageBox.Show("Данные успешно обновлены", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Обновление в БД не было выполнено из-за некорректно указанных данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tab = Connection.ds.Tables["Услуга"];
                string message = $"Вы точно хотите удалить услугу \n{tab.Rows[n]["name"].ToString()} ?";
                string caption = "Удаление услуги";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxImage image = MessageBoxImage.Warning;
                MessageBoxResult rezult = MessageBox.Show(message, caption, buttons, image);
                if (rezult == MessageBoxResult.No) { return; }
                string id = tab.Rows[n]["id"].ToString();
                sql = $"DELETE FROM service WHERE id={id};";
                if (!Connection.Modification_Execute(sql))
                    return;
                Connection.ds.Tables["Услуга"].Rows.RemoveAt(n);
                if (Connection.ds.Tables["Услуга"].Rows.Count > n)
                    FieldsForm_Fill();
                else
                {
                    n--;
                    FieldsForm_Fill();
                }

                MessageBox.Show("Удаление прошло успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Удаление было не выполнено", "Ошибка");
            }
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            if (n > 0)
            {
                n--;
                FieldsForm_Fill();
            }
        }

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            n = 0;
            FieldsForm_Fill();

        }

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            if (n < Connection.ds.Tables["Услуга"].Rows.Count) n++;
            if (Connection.ds.Tables["Услуга"].Rows.Count > n)
                FieldsForm_Fill();
        }

        private void btn_End_Click(object sender, RoutedEventArgs e)
        {
            n = Connection.ds.Tables["Услуга"].Rows.Count - 1;
            FieldsForm_Fill();
        }
    }
}
