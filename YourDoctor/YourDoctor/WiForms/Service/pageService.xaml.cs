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
        Generate generate = new Generate(); 
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
                DataRowView selectedRow = nurseDataGrid.SelectedItem as DataRowView;

                if (selectedRow != null)
                {
                    int selectedId = Convert.ToInt32(selectedRow["id"]);
                    DataRowView foundRow = generate.FindRowById(selectedId, "Услуга");

                    if (foundRow != null)
                    {
                        int selectedRowIndex = Connection.ds.Tables["Услуга"].Rows.IndexOf(foundRow.Row);

                        this.NavigationService.Navigate(new customerService(selectedRowIndex));
                    }
                }
            }

        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtFilter.Text;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                nurseDataGrid.ItemsSource = Connection.ds.Tables["Услуга"].DefaultView;
            }
            else
            {
                var filteredView = new DataView(Connection.ds.Tables["Услуга"]);
                filteredView.RowFilter = $"name LIKE '%{searchText}%'";

                nurseDataGrid.ItemsSource = filteredView;
            }
        }
    }
}
