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
using YourDoctor.WiForms.Administrator.objDoctor;

namespace YourDoctor.WiForms.Patient
{
    /// <summary>
    /// Логика взаимодействия для customerPatient.xaml
    /// </summary>
    public partial class customerPatient : Page
    {
        public int n;

        public customerPatient(int id)
        {
            InitializeComponent();
            Dictionary<char, string> gender = new Dictionary<char, string>()
            {
                {'М',"Мужской" },
                {'Ж',"Женский" },
            };
            comboBox1.ItemsSource = gender;
            n = id;
            if (Connection.ds.Tables["Пациент"].Rows.Count > n)
                FieldsForm_Fill();
            switch(App.RoleUsers)
            {
                case "Администратор":
                    break;
                default:
                    btn_Delete.Visibility = Visibility.Hidden;
                    btn_Save.Width = btn_Save.Width * 2.3;
                    break;
            }
        }

        private void FieldsForm_Fill()
        {
            using (var tab = Connection.ds.Tables["Пациент"])
            {
                txtFamily.Text = tab.Rows[n]["family"].ToString();
                txtImy.Text = tab.Rows[n]["imy"].ToString();
                txtOtchestvo.Text = tab.Rows[n]["otchestvo"].ToString();
                datePicker.Text = tab.Rows[n]["birth_date"].ToString();
                comboBox1.SelectedValue = tab.Rows[n]["sex"].ToString();
                txtAddress.Text = tab.Rows[n]["address"].ToString();
                txtNumberPhone.Text = tab.Rows[n]["phone"].ToString();
            }
        }

        private void btn_ReturnPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new pagePatient());
        }

        string sql;
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
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
                string f = txtFamily.Text;
                string i = txtImy.Text;
                string o = txtOtchestvo.Text;
                string id = Connection.ds.Tables["Пациент"].Rows[n]["id"].ToString();
                sql = $"UPDATE patient SET family='{f}', imy='{i}', otchestvo='{o}'," +
                $" address='{txtAddress.Text}',phone='{txtNumberPhone.Text}', birth_date='{datePicker.Text}', " +
                $"sex='{comboBox1.SelectedValue.ToString()}' WHERE id={id};";

                if (!Connection.Modification_Execute(sql))
                    return;

                Connection.ds.Tables["Пациент"].Rows[n].ItemArray = new object[] { id, f, i, o, txtAddress.Text, txtNumberPhone.Text, datePicker.Text, comboBox1.SelectedValue.ToString() };
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
                var tab = Connection.ds.Tables["Пациент"];
                string message = $"Вы точно хотите удалить пациента \n{tab.Rows[n]["family"].ToString()} " +
                    $"{tab.Rows[n]["imy"].ToString()} {tab.Rows[n]["otchestvo"].ToString()}?";
                string caption = "Удаление пациента";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxImage image = MessageBoxImage.Warning;
                MessageBoxResult rezult = MessageBox.Show(message, caption, buttons, image);
                if (rezult == MessageBoxResult.No) { return; }
                string id = tab.Rows[n]["id"].ToString();
                sql = $"DELETE FROM patient WHERE id={id};";
                if (!Connection.Modification_Execute(sql))
                    return;
                Connection.ds.Tables["Пациент"].Rows.RemoveAt(n);
                if (Connection.ds.Tables["Пациент"].Rows.Count > n)
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
            if (n < Connection.ds.Tables["Пациент"].Rows.Count) n++;
            if (Connection.ds.Tables["Пациент"].Rows.Count > n)
                FieldsForm_Fill();
        }

        private void btn_End_Click(object sender, RoutedEventArgs e)
        {
            n = Connection.ds.Tables["Пациент"].Rows.Count - 1;
            FieldsForm_Fill();
        }
    }
}
