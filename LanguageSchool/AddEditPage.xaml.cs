﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace LanguageSchool
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    /// 

    public partial class AddEditPage : Page
    {
        Client currentClientGlobal = new Client();
        List<Client> clients = LanguageSHEntities.getContext().Client.ToList();
        bool isNew = false;

        public AddEditPage(Client currentClient)
        {
            InitializeComponent();

            if (currentClient != null)
                currentClientGlobal = currentClient;

            DataContext = currentClientGlobal;
            if (currentClientGlobal.GenderCode == "м")
            {
                genderMale.IsChecked = true;
            }

            if (currentClientGlobal.GenderCode == "ж")
            {
                genderFemale.IsChecked = true;
            }

            if (currentClient == null)
            {
                foreach (var item in clients)
                {
                    currentClientGlobal.ID = item.ID;
                }
                currentClientGlobal.ID++;
                currentClientGlobal.RegistrationDate = DateTime.Now;
                currentClientGlobal.PhotoPath = "img/school_logo.png";
                LanguageSHEntities.getContext().Client.Add(currentClientGlobal);
                isNew = true;

            }

            if (isNew)
            {
                id.Visibility = Visibility.Hidden;
                idMeta.Visibility = Visibility.Hidden;
            }
        }


        private void genderMale_Checked(object sender, RoutedEventArgs e)
        {
            currentClientGlobal.GenderCode = "м";
        }

        private void genderFemale_Checked(object sender, RoutedEventArgs e)
        {
            currentClientGlobal.GenderCode = "ж";

        }

        private void ChangePicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == true)
            {
                currentClientGlobal.PhotoPath = openFileDialog1.FileName;
                Avatar.Source = new BitmapImage(new Uri(openFileDialog1.FileName));
            }
        }

        private bool isEmail(string email)
        {
            var emailParts = email.Split('@');
            if (emailParts.Length != 2)
            {
                return false;
            }
            var domain = emailParts[1].Split('.');
            if (domain[0].Length < 2 || domain[1].Length < 2) return false;
            if (emailParts[1].EndsWith(".")) return false;

            var cyryllic = Enumerable.Range(1024, 256).Select(ch => (char)ch);

            //! $ &*- = \^ ` | ~ # % ‘ + / ? _ { }
            if (email.Any(cyryllic.Contains) ||
                email.StartsWith("@") || email.Contains('!') || email.Contains('$') || email.Contains('&')
                || email.Contains('\'') || email.Contains('+') || email.Contains('%') || email.Contains('=') || email.Contains('}') || email.Contains('{')
                || email.EndsWith(".") || email.EndsWith("@") || email.StartsWith(".")
                ) return false;
            return true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            var fullname = currentClientGlobal.LastName + " " + currentClientGlobal.FirstName + " " + currentClientGlobal.Patronymic;
            if (string.IsNullOrWhiteSpace(currentClientGlobal.LastName) ||
                string.IsNullOrWhiteSpace(currentClientGlobal.FirstName) ||
                string.IsNullOrWhiteSpace(currentClientGlobal.Patronymic))
                errors.AppendLine("Введите ваше полное ФИО");
            else if (currentClientGlobal.LastName.Length > 50 ||
                currentClientGlobal.Patronymic.Length > 50 ||
                currentClientGlobal.FirstName.Length > 50 ||
                fullname.Contains('1') || fullname.Contains('2') || fullname.Contains('3') || fullname.Contains('4') || fullname.Contains('5') ||
                fullname.Contains('9') || fullname.Contains('8') || fullname.Contains('7') || fullname.Contains('6') || fullname.Contains('0') ||
                fullname.Contains('!') || fullname.Contains('$') || fullname.Contains('&')
                || fullname.Contains('\'') || fullname.Contains('+') || fullname.Contains('%') || fullname.Contains('=') || fullname.Contains('}') || fullname.Contains('{')
                 )
                errors.AppendLine("Неверное ФИО");


            //if (currentClientGlobal.LastName.Contains('1', '2', '3', '4', '5', '6', '7', '8', '9', '0'))
            //    errors.AppendLine("ФИО не может содержать цифр");




            

            if (string.IsNullOrWhiteSpace(currentClientGlobal.Email))
                errors.AppendLine("Введите e-mail");
            else if (
                (!currentClientGlobal.Email.Contains('@') || !currentClientGlobal.Email.Contains('.')) || !isEmail(currentClientGlobal.Email)
                )
            {
                errors.AppendLine("Неправильный e-mail");
            }

    

            if (string.IsNullOrWhiteSpace(currentClientGlobal.Birthday.ToString()))
            {
                errors.AppendLine("Укажите дату рождения");
            }

            if (string.IsNullOrWhiteSpace(currentClientGlobal.Phone))
                errors.AppendLine("Укажите номер телефона");
            else
            {
                string phone = currentClientGlobal.Phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("+", "");
                if ((phone[1] == '9' || phone[1] == '4' || phone[1] == '8') && phone.Length != 11)
                {
                    errors.AppendLine("Номер телефона указан неверно");
                }
            }

            

            if (string.IsNullOrWhiteSpace(currentClientGlobal.Email))
                errors.AppendLine("Укажите е-мейл");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (currentClientGlobal.PhotoPath.Contains(":"))
            {
                var pathParts = currentClientGlobal.PhotoPath.Split('\\');
                currentClientGlobal.PhotoPath = pathParts[pathParts.Length - 2] + '\\' + pathParts[pathParts.Length - 1];
            }

            if (isNew)
            {
                LanguageSHEntities.getContext().Client.Add(currentClientGlobal);
            }

           

            try
            {


                LanguageSHEntities.getContext().SaveChanges();
                MessageBox.Show("Успешно сохранено ");
                Manager.MainFrame.GoBack();
                UpdateLayout();
              
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
