﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


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
            string sql;
            var nurse = Connection.ds.Tables["Медсестра"];
            var authoriz = Connection.ds.Tables["Авторизация"];
            var id = Convert.ToInt32(nurse.Rows[nurse.Rows.Count-1]["id"])+1;
            var ids = Convert.ToInt32(authoriz.Rows[authoriz.Rows.Count-1]["id"])+1;
            string login = Generate.Transliterate($"{txtFamily.Text} {txtImy.Text.Substring(0, 1)}.{txtOtchestvo.Text.Substring(0, 1)}.");
            string password = Generate.Password(Generate.Transliterate(txtFamily.Text));
            try
            {
                string formattedPhoneNumber = Generate.FormatPhoneNumber(txtNumberPhone.Text);

                txtNumberPhone.Text = formattedPhoneNumber;
            }
            catch (ArgumentException ex)
            {
                // Выводим сообщение об ошибке в формате номера телефона
                MessageBox.Show($"Ошибка: {ex.Message}","Ошибка");
                return;
            }

            try
            {
                //sql = "INSERT INTO nurse " +
                //    $"VALUES ({id}, '{txtFamily.Text}', '{txtImy.Text}', '{txtOtchestvo.Text}', '{txtAddress.Text}', '{txtNumberPhone.Text}', '{datePicker.Text}', '{comboBox1.SelectedValue.ToString()}');";
                sql = "BEGIN TRANSACTION; " +
      $"INSERT INTO nurse VALUES ({id}, '{txtFamily.Text}', '{txtImy.Text}', '{txtOtchestvo.Text}', '{txtAddress.Text}', '{txtNumberPhone.Text}', '{datePicker.Text}', '{comboBox1.SelectedValue.ToString()}'); " +
      $"INSERT INTO \"authorization\" VALUES ({ids}, '{login}', " +
      $"'{Generate.transHash(password)}','Медсестра',NULL,{id}); " +
      "COMMIT;";
                if (!Connection.Modification_Execute(sql))
                    return;
                MessageBox.Show("Добавлена новая медсестра!!! \n Передайте ей следующий логин и пароль:\n" +
                    $"Логин: {login}\n Пароль: {password}","Успешно",MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception ex) 
            {
                MessageBox.Show($"{ex}", "Ошибка", MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            nurse.Rows.Add(new object[] { id, txtFamily.Text + " " + txtImy.Text +" "+ txtOtchestvo.Text, comboBox1.SelectedValue.ToString(), datePicker.Text, txtAddress.Text, txtNumberPhone.Text});



            this.NavigationService.Navigate(new pageNurse());
            //var slovo = Generate.Transliterate(txtFamily.Text);
            //txtImy.Text = slovo;   
            //txtOtchestvo.Text = Generate.Password(slovo);
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

