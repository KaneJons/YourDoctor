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
    /// Логика взаимодействия для pageService.xaml
    /// </summary>
    public partial class pageService : Page
    {
        public pageService()
        {
            InitializeComponent();
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "select * from service order by id asc;";
            Connection.Table_Fill("Услуга", sql);
            nurseDataGrid.ItemsSource = Connection.ds.Tables["Услуга"].DefaultView;

            nurseDataGrid.SelectionMode = (DataGridSelectionMode)DataGridSelectionUnit.FullRow;
        }

        private void btn_addLog_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new addService());
        }

        private void nurseDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nurseDataGrid.SelectedItem != null) // Если выбрана строка
            {
                this.NavigationService.Navigate(new customerService(nurseDataGrid.SelectedIndex));
            }

        }
    }
}
