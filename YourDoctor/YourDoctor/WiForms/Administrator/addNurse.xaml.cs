using Npgsql;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace YourDoctor.WiForms.Administrator
{
    /// <summary>
    /// Логика взаимодействия для addNurse.xaml
    /// </summary>
    public partial class addNurse : Page
    {

        public addNurse()
        {
            InitializeComponent();
            Dictionary<char,string> gender= new Dictionary<char, string>()
            {
                {'М',"Мужской" },
                {'Ж',"Женский" },
            };
            comboBox1.ItemsSource = gender;
            
        }

        private void btn_ReturnPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new pageNurse());
        }


        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            //string sql;
            //try
            //{
            //    sql = "INSERT INTO nurse VALUES " +
            //        "({}";
            //}
            //catch(Exception ex) 
            //{
            //    MessageBox.Show($"{ex}", "Ошибка");
            //}

            var slovo = Generate.Transliterate(txtFamily.Text);
            txtImy.Text = slovo;   
            txtOtchestvo.Text = Generate.Password(slovo);
        }

        private const string defaultText = "Пример: +7 987 654-32-10";

        private void txtNumberPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            txtNumberPhone.Text = txtNumberPhone.Text == defaultText ? string.Empty : txtNumberPhone.Text;

        }

        private void txtNumberPhone_LostFocus(object sender, RoutedEventArgs e)
        {
            txtNumberPhone.Text = txtNumberPhone.Text == string.Empty ? defaultText : txtNumberPhone.Text;
        }


    }

    }

