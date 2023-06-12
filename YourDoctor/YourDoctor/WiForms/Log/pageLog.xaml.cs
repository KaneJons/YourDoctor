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
using System.Windows.Navigation;
using System.Windows.Shapes;
using YourDoctor.WiForms.Patient;

namespace YourDoctor.WiForms.Log
{
    /// <summary>
    /// Логика взаимодействия для pageLog.xaml
    /// </summary>
    public partial class pageLog : Page
    {
        public pageLog()
        {
            InitializeComponent();
        }
        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT appointment.id, CONCAT(doctor.family, ' ', doctor.imy, ' ', doctor.otchestvo) AS doctor_fio," +
                "CONCAT(patient.family, ' ', patient.imy, ' ', patient.otchestvo) AS patient_fio," +
                "service.name AS service_name, appointment.date, appointment.time, appointment.status, appointment.treatment " +
                "FROM appointment " +
                "JOIN doctor ON appointment.doctor_id = doctor.id " +
                "JOIN patient ON appointment.patient_id = patient.id " +
                "JOIN service ON appointment.service_id = service.id;";
            Connection.Table_Fill("Запись", sql);
            nurseDataGrid.ItemsSource = Connection.ds.Tables["Запись"].DefaultView;

            nurseDataGrid.SelectionMode = (DataGridSelectionMode)DataGridSelectionUnit.FullRow;
        }

        private void nurseDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            

        }
    }
}
