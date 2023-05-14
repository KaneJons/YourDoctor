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

namespace YourDoctor.WiForms.Administrator.objDoctor
{
    /// <summary>
    /// Логика взаимодействия для pageDoctor.xaml
    /// </summary>
    public partial class pageDoctor : Page
    {
        public pageDoctor()
        {
            InitializeComponent();
        }
        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "select * from doctor order by id asc;";
            Connection.Table_Fill("Доктор", sql);
            nurseDataGrid.ItemsSource = Connection.ds.Tables["Доктор"].DefaultView;

            nurseDataGrid.SelectionMode = (DataGridSelectionMode)DataGridSelectionUnit.FullRow;
        }




        private void btn_addLog_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new addDoctor());
        }

        private void nurseDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nurseDataGrid.SelectedItem != null) // Если выбрана строка
            {
                this.NavigationService.Navigate(new customerDoctor(nurseDataGrid.SelectedIndex));
            }

        }
    }
}
