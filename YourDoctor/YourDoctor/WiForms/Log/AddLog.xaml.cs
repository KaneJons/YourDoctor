using Npgsql;
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
using YourDoctor.WiForms.Other;
using YourDoctor.WiForms.Patient;

namespace YourDoctor.WiForms.Log
{
    /// <summary>
    /// Логика взаимодействия для AddLog.xaml
    /// </summary>
    public partial class AddLog : Page
    {
        public string selectedpatid,selecteddocid, selectedserid;
        public AddLog()
        {
            InitializeComponent();
            comboBox1.Items.Clear();
            comboBox1.Items.Add("");
            comboBox1.Items.Add("Запись");
            comboBox1.Items.Add("Посещение");
            comboBox1.Items.Add("Отмена");
        }

        private void btn_ReturnPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new pageLog());
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            try
            {
                sql = $"INSERT INTO appointment values ({txtID.Text},{selecteddocid},{selectedpatid},{selectedserid},'{datePicker.Text}','{NumericHourse.Value.ToString()}:{NumericMinutes.Value.ToString()}','{comboBox1.Text}','{txtDESCRIPTION.Text}');";
                if (!Connection.Modification_Execute(sql))
                    return;
                MessageBox.Show("Успешно добавлена новая запись!!!","Успешно",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            catch (NpgsqlException ex)
            {
                    MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           

            this.NavigationService.Navigate(new pageLog());
        }

        private void btn_ReturnPage_Copy_Click(object sender, RoutedEventArgs e)
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
