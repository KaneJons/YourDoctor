using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using YourDoctor.WiForms.Other;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace YourDoctor.WiForms.Log
{
    /// <summary>
    /// Логика взаимодействия для CustomerLog.xaml
    /// </summary>
    public partial class CustomerLog : Page
    {
        public string selectedpatid, selecteddocid, selectedserid;
        public int n;
        public CustomerLog(int id)
        {
            InitializeComponent();
            comboBox1.Items.Clear();
            comboBox1.Items.Add("");
            comboBox1.Items.Add("Запись");
            comboBox1.Items.Add("Посещение");
            comboBox1.Items.Add("Отмена");
            selecteddocid = ""; selectedpatid = "";selectedserid="";

            n = id;

            if (Connection.ds.Tables["Запись"].Rows.Count > n) { 
            using (var tab = Connection.ds.Tables["Запись"])
            {
                txtID.Text = tab.Rows[n]["id"].ToString();
                txtFIO.Text = tab.Rows[n]["patient_fio"].ToString();
                txtDoctor.Text = tab.Rows[n]["doctor_fio"].ToString();
                txtService.Text = tab.Rows[n]["service_name"].ToString();
                comboBox1.Text = tab.Rows[n]["status"].ToString();
                datePicker.Text = tab.Rows[n]["date"].ToString();
                txtDESCRIPTION.Text = tab.Rows[n]["treatment"].ToString();
                var timer = tab.Rows[n]["time"].ToString();
                DateTime time = DateTime.ParseExact(timer, "HH:mm:ss", CultureInfo.InvariantCulture);
                NumericHourse.Value = time.Hour;
                NumericMinutes.Value =time.Minute;
                }
            }
        }



        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string message = $"Вы точно хотите удалить запись с кодом {txtID.Text}?";
                string caption = "Удаление врача";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxImage image = MessageBoxImage.Warning;
                MessageBoxResult rezult = MessageBox.Show(message, caption, buttons, image);
                if (rezult == MessageBoxResult.No) { return; }
                if (!Connection.Modification_Execute($"DELETE FROM appointment WHERE id={txtID.Text};"))
                    return;

                MessageBox.Show("Удаление прошло успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.NavigationService.Navigate(new pageLog());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Удаление было не выполнено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
           if(selecteddocid=="" || selectedpatid=="" || selectedserid=="")
            {
                Connection.Table_Fill("oldid", $"SELECT doctor_id,patient_id,service_id from appointment where id={txtID.Text};");
            if (selectedpatid=="")
            {
                        selectedpatid = Connection.ds.Tables["oldid"].Rows[0]["patient_id"].ToString();
            }
           if (selecteddocid=="")
            {
                        selecteddocid = Connection.ds.Tables["oldid"].Rows[0]["doctor_id"].ToString();
                    }
           if (selectedserid=="")
            {
                        selectedserid = Connection.ds.Tables["oldid"].Rows[0]["service_id"].ToString();
                    }
            }
                string sql = $"UPDATE appointment SET doctor_id={selecteddocid},patient_id={selectedpatid},service_id={selectedserid},date='{datePicker.Text}',time='{NumericHourse.Value.ToString()}:{NumericMinutes.Value.ToString()}',status='{comboBox1.Text}',treatment='{txtDESCRIPTION.Text}' where id = {txtID.Text};";
                if (!Connection.Modification_Execute(sql))
                    return;
                MessageBox.Show("Данные успешно обновлены", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка:","Ошибка");
                return;
            }
        }

        private void btn_ReturnPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new pageLog());
        }

        private void btn_choose_patient_Click(object sender, RoutedEventArgs e)
        {
            formPatient formPatient = new formPatient();
            formPatient.PatientSelected += FormPatient_PatientSelected;
            formPatient.ShowDialog();
        }

        private void FormPatient_PatientSelected(string selectedFio, string selectedId)
        {
            txtFIO.Text = selectedFio;
            selectedpatid = selectedId;
        }

        private void btn_choose_doctor_Click(object sender, RoutedEventArgs e)
        {
            formDoctor formDoctor = new formDoctor();
            formDoctor.DoctorSelected += FormDoctor_DoctorSelected;
            formDoctor.ShowDialog();
        }

        private void FormDoctor_DoctorSelected(string selectedFio, string selectedId)
        {
            txtDoctor.Text = selectedFio;
            selecteddocid = selectedId;
        }

        private void btn_choose_service_Click(object sender, RoutedEventArgs e)
        {
            formService formService = new formService();
            formService.ServiceSelected += FormService_ServiceSelected;
            formService.ShowDialog();
        }
        private void FormService_ServiceSelected(string selectedName, string selectedId)
        {
            txtService.Text = selectedName;
            selectedserid = selectedId;
        }
    }
}
