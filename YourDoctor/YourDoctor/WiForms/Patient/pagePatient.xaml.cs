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
    /// Логика взаимодействия для pagePatient.xaml
    /// </summary>
    public partial class pagePatient : Page
    {
       
        public pagePatient()
        {
            InitializeComponent();
            
        }
        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "select * from patient order by id asc;";
            Connection.Table_Fill("Пациент", sql);
            nurseDataGrid.ItemsSource = Connection.ds.Tables["Пациент"].DefaultView;

            nurseDataGrid.SelectionMode = (DataGridSelectionMode)DataGridSelectionUnit.FullRow;
        }




        private void btn_addLog_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new addPatient());
        }

        private void nurseDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

           
            if (nurseDataGrid.SelectedItem != null) // Если выбрана строка
            {
                this.NavigationService.Navigate(new customerPatient(nurseDataGrid.SelectedIndex));
            }

        }
    }
}
