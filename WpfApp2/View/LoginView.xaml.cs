using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public static class AppDataContext
    {
        public static LanguageLearningApplicationDataContext context = new LanguageLearningApplicationDataContext();
    }
    public static class UserProfile
    {
      static  public User user;
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
            string hashedPassword = ComputeHash(ConvertSecureStringToString(txtBoxPassword.SecurePassword));
            var user = (from u in AppDataContext.context.Users where u.Email.Equals(txtBoxUser.Text) select u).FirstOrDefault();
            
            if (user != null && hashedPassword == user.Password)
            {
                UserProfile.user = user;
                if(UserProfile.user.Role == "Educator")
                {
                    HomeEducator homeEd = new HomeEducator();
                    homeEd.Show();
                }
                else
                {
                    Home home = new Home(user.FirstName, user.LastName);
                    home.Show();
                }
                
                user.LastLoginDate= DateTime.Now;
                AppDataContext.context.SubmitChanges();
                
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
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Register reg = new Register();
            reg.Show();
            Close();
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            RecoverPasswordView rec = new RecoverPasswordView();
            rec.Show();
            Close();
        }
    }
}
