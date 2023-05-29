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
using YourDoctor.WiForms.Log;
using YourDoctor.WiForms.Patient;

namespace YourDoctor.WiForms.Doctor
{
    /// <summary>
    /// Логика взаимодействия для Doctor.xaml
    /// </summary>
    public partial class Doctor : Window
    {
        public Doctor(string name)
        {
            InitializeComponent();
            textBlock1.Text = name;
            App.RoleUsers = textBlock2.Text;
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

        private void btn_patients_Click(object sender, RoutedEventArgs e)
        {
            txtBlock.Text = "Пациент";
            MyFrame.NavigationService.Navigate(new pagePatient());
        }

        private void btn_lesson_log_Click(object sender, RoutedEventArgs e)
        {
            txtBlock.Text = "Журнад записи к врачу";
            MyFrame.NavigationService.Navigate(new pageLog());
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
    }
}
