using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using YourDoctor.WiForms.Patient;

namespace YourDoctor.WiForms.Administrator
{
    /// <summary>
    /// Логика взаимодействия для pageNurse.xaml
    /// </summary>
    public partial class pageNurse : Page
    {
        public pageNurse()
        {
            InitializeComponent();
        }
         public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "select id, concat(family, ' ', imy, ' ', otchestvo) as fio, sex, birth_date, address, phone  from nurse order by id asc;"; 
            Connection.Table_Fill("Медсестра",sql);
            nurseDataGrid.ItemsSource = Connection.ds.Tables["Медсестра"].DefaultView;
           
            nurseDataGrid.SelectionMode = (DataGridSelectionMode)DataGridSelectionUnit.FullRow;
        }

        
      Generate generate = new Generate();

        private void btn_addLog_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new addNurse());
        }

        private void nurseDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (nurseDataGrid.SelectedItem != null) // Если выбрана строка
            {
                DataRowView selectedRow = nurseDataGrid.SelectedItem as DataRowView;

                if (selectedRow != null)
                {
                    int selectedId = Convert.ToInt32(selectedRow["id"]);
                    DataRowView foundRow = generate.FindRowById(selectedId, "Медсестра");

                    if (foundRow != null)
                    {
                        int selectedRowIndex = Connection.ds.Tables["Медсестра"].Rows.IndexOf(foundRow.Row);

                        this.NavigationService.Navigate(new customerNurse(selectedRowIndex));
                    }
                }
            }

        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtFilter.Text;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                nurseDataGrid.ItemsSource = Connection.ds.Tables["Медсестра"].DefaultView;
            }
            else
            {
                var filteredView = new DataView(Connection.ds.Tables["Медсестра"]);
                filteredView.RowFilter = $"fio LIKE '%{searchText}%'";

                nurseDataGrid.ItemsSource = filteredView;
            }
        }
    }
}
