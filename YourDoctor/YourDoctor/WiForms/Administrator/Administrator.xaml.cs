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
using System.Windows.Shapes;

namespace YourDoctor.WiForms.Administrator
{
    /// <summary>
    /// Логика взаимодействия для Administrator.xaml
    /// </summary>
    public partial class Administrator : Window
    {
        public Administrator()
        {
            InitializeComponent();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
           
           Authorization_window authorization_Window = new Authorization_window();
            authorization_Window.Show();
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

        private void btn_service_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_specialist_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_nurse_Click(object sender, RoutedEventArgs e)
        {
            txtBlock.Text = "Медсестра/Медбрат";
            MyFrame.NavigationService.Navigate(new pageNurse());
        }   
        private void btn_patients_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_lesson_log_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

