﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
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

namespace WpfApp2.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    /// 
    public static class AppDataContext
    {
        public static LanguageLearningApplicationDataContext context = new LanguageLearningApplicationDataContext();
    }
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string parolaIntrodusa = ConvertSecureStringToString(txtBoxPassword.SecurePassword);
            string hashedPassword = ComputeHash(parolaIntrodusa);
            var user = (from u in AppDataContext.context.Users where u.Email.Equals(txtBoxUser.Text) select u).FirstOrDefault();
            if (user != null && hashedPassword == user.Password)
            {
                Home home = new Home(user.FirstName, user.LastName);
                home.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Adresa de email sau parola gresite. Va rugam incercati din nou.", "Eroare de autentificare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string ConvertSecureStringToString(SecureString secureString)
        {
            IntPtr ptr = Marshal.SecureStringToBSTR(secureString);
            try
            {
                return Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
        }
        private string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
