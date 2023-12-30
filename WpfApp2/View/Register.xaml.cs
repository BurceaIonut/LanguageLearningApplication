using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
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

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var user = (from u in AppDataContext.context.Users where u.Email.Equals(txtBoxUser.Text)  select u).FirstOrDefault();
            if(user != null)
            {
                MessageBox.Show("Acest utilizator exista deja! Va rugam incercati alta adresa de email.", "Inregistrare esuata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string hashedPassword = ComputeHash(ConvertSecureStringToString(txtBoxPassword.SecurePassword));
            var newUser = new User
            {
                Password = hashedPassword,
                Email = txtBoxUser.Text,
                FirstName = txtBoxUserFName.Text,
                LastName = txtBoxUserLName.Text,
                RegistrationDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                Role = "Member"
            };
            AppDataContext.context.Users.InsertOnSubmit(newUser);
            AppDataContext.context.SubmitChanges();
            Home home = new Home(txtBoxUserFName.Text, txtBoxUserLName.Text);
            home.Show();
            Close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            LoginView lg = new LoginView();
            lg.Show();
            Close();
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
