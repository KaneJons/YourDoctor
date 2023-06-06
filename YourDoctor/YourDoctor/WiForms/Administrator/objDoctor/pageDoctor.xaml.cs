using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace YourDoctor.WiForms.Administrator.objDoctor
{
    /// <summary>
    /// Логика взаимодействия для pageDoctor.xaml
    /// </summary>
    public partial class pageDoctor : System.Windows.Controls.Page
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

        Generate generate = new Generate();


        private void btn_addLog_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new addDoctor());
        }

        private void nurseDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (nurseDataGrid.SelectedItem != null) // Если выбрана строка
            {
                DataRowView selectedRow = nurseDataGrid.SelectedItem as DataRowView;

                if (selectedRow != null)
                {
                    int selectedId = Convert.ToInt32(selectedRow["id"]);
                    DataRowView foundRow = generate.FindRowById(selectedId, "Доктор");

                    if (foundRow != null)
                    {
                        int selectedRowIndex = Connection.ds.Tables["Доктор"].Rows.IndexOf(foundRow.Row);

                        this.NavigationService.Navigate(new customerDoctor(selectedRowIndex));
                    }
                }
            }

        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtFilter.Text;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                nurseDataGrid.ItemsSource = Connection.ds.Tables["Доктор"].DefaultView;
            }
            else
            {
                var filteredView = new DataView(Connection.ds.Tables["Доктор"]);
                filteredView.RowFilter = $"family LIKE '%{searchText}%' OR imy LIKE '%{searchText}%' OR otchestvo LIKE '%{searchText}%' OR (family + ' ' + imy + ' ' + otchestvo) LIKE '%{searchText}%'";

                nurseDataGrid.ItemsSource = filteredView;
            }
        }
    }
}
