using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace YourDoctor.WiForms.Administrator
{
    /// <summary>
    /// Логика взаимодействия для customerNurse.xaml
    /// </summary>
    public partial class customerNurse : Page
    {
        public int n;
        string[] fio;
        public customerNurse(int id)
        {
            InitializeComponent();
            Dictionary<char, string> gender = new Dictionary<char, string>()
            {
                {'М',"Мужской" },
                {'Ж',"Женский" },
            };
            comboBox1.ItemsSource = gender;
            n = id;
            if (Connection.ds.Tables["Медсестра"].Rows.Count > n)
                FieldsForm_Fill();
        }

        private void FieldsForm_Fill()
        {
            fio  = Connection.ds.Tables["Медсестра"].Rows[n]["fio"].ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            txtFamily.Text = fio[0];
            txtImy.Text = fio[1];
            txtOtchestvo.Text = fio[2];
            datePicker.Text = Connection.ds.Tables["Медсестра"].Rows[n]["birth_date"].ToString();
            comboBox1.SelectedValue = Connection.ds.Tables["Медсестра"].Rows[n]["sex"].ToString();
            txtAddress.Text = Connection.ds.Tables["Медсестра"].Rows[n]["address"].ToString();
            txtNumberPhone.Text = Connection.ds.Tables["Медсестра"].Rows[n]["phone"].ToString();
        }

        private void btn_ReturnPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new pageNurse());
        }

        string sql;
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string id = Connection.ds.Tables["Медсестра"].Rows[n]["id"].ToString();
                sql = "BEGIN TRANSACTION; " +
                    $"UPDATE nurse SET family='{txtFamily.Text}', imy='{txtImy.Text}', otchestvo='{txtOtchestvo.Text}'," +
                $" address='{txtAddress.Text}',phone='{txtNumberPhone.Text}', birth_date='{datePicker.Text}', " +
                $"sex='{comboBox1.SelectedValue.ToString()}' WHERE id={id};" +
                $"UPDATE \"authorization\" SET login='{Generate.Transliterate($"{txtFamily.Text} {txtImy.Text.Substring(0, 1)}.{txtOtchestvo.Text.Substring(0, 1)}.")}' WHERE nurse_id={id};" +
                "COMMIT;";
            if (!Connection.Modification_Execute(sql))
                return;

            Connection.ds.Tables["Медсестра"].Rows[n].ItemArray = new object[] { id, txtFamily.Text + " " + txtImy.Text + " " + txtOtchestvo.Text, comboBox1.SelectedValue.ToString(), datePicker.Text, txtAddress.Text, txtNumberPhone.Text };
            Connection.Table_Fill("Авторизация", "select *from \"authorization\" order by id asc;");
                MessageBox.Show("Данные успешно обновлены","Успешно",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Обновление в БД не было выполнено из-за некорректно указанных данных", "Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string message = $"Вы точно хотите удалить медсестру/медбрата \n{fio[0]} {fio[1]} {fio[2]}?";
                string caption = "Удаление медсестры/медбрата";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxImage image = MessageBoxImage.Warning;
                MessageBoxResult rezult = MessageBox.Show(message, caption, buttons, image);
                if (rezult == MessageBoxResult.No) { return; }
                string id = Connection.ds.Tables["Медсестра"].Rows[n]["id"].ToString();
                sql = "BEGIN TRANSACTION; " +
            $"DELETE FROM \"authorization\" WHERE nurse_id={id};" +
            $"DELETE FROM nurse WHERE id={id};" +
                "COMMIT;";
                if (!Connection.Modification_Execute(sql))
                    return;
                Connection.Table_Fill("Авторизация", "select *from \"authorization\" order by id asc;");
                Connection.ds.Tables["Медсестра"].Rows.RemoveAt(n);
                if (Connection.ds.Tables["Медсестра"].Rows.Count > n)
                    FieldsForm_Fill();
                else
                {
                    n--;
                    FieldsForm_Fill();
                }

                MessageBox.Show("Удаление прошло успешно", "Успех", MessageBoxButton.OK,MessageBoxImage.Information);

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
            if (n < Connection.ds.Tables["Медсестра"].Rows.Count) n++;
            if (Connection.ds.Tables["Медсестра"].Rows.Count > n)
                FieldsForm_Fill();
        }

        private void btn_End_Click(object sender, RoutedEventArgs e)
        {
           n = Connection.ds.Tables["Медсестра"].Rows.Count-1;
            FieldsForm_Fill();
        }
    }
}
