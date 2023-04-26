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
            string sql = "select id, concat(family, ' ', imy, ' ', otchestvo) as fio, sex, birth_date, address, phone  from nurse;"; 
            Connection.Table_Fill("Медсестра",sql);
            nurseDataGrid.ItemsSource = Connection.ds.Tables["Медсестра"].DefaultView;
           
            nurseDataGrid.SelectionMode = (DataGridSelectionMode)DataGridSelectionUnit.FullRow;
        }

        
      

        private void btn_addLog_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new addNurse());
        }
    }
}
