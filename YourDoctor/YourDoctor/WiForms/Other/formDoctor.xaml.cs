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
using System.Windows.Shapes;

namespace YourDoctor.WiForms.Other
{
    /// <summary>
    /// Логика взаимодействия для formDoctor.xaml
    /// </summary>
    public partial class formDoctor : Window
    {
        public formDoctor()
        {
            InitializeComponent();

            string sql = "select * from doctor order by id asc;";
            Connection.Table_Fill("Доктор", sql);
            membersDataGrid.ItemsSource = Connection.ds.Tables["Доктор"].DefaultView;

            membersDataGrid.SelectionMode = (DataGridSelectionMode)DataGridSelectionUnit.FullRow;
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private bool IsMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximized = true;
                }
            }
        }

        WindowState oldState;
        private void btn_maximized_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                oldState = this.WindowState;

                this.WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = oldState;
            }

        }

        private void btn_roll_up_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;

        }

        public event Action<string, string> DoctorSelected;

        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (membersDataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)membersDataGrid.SelectedItem;
                string selectedFio = selectedRow["family"].ToString() + " " + selectedRow["imy"].ToString() + " " + selectedRow["otchestvo"].ToString();

                DoctorSelected?.Invoke(selectedFio, selectedRow["id"].ToString());

                this.Close();
            }
        }
    }
}
