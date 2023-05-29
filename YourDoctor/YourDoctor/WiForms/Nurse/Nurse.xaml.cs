using System.Windows;
using System.Windows.Input;
using YourDoctor.WiForms.Patient;
using YourDoctor.WiForms.Log;

namespace YourDoctor.WiForms.Nurse
{
    public partial class Nurse : Window
    {
        public Nurse(string name)
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
