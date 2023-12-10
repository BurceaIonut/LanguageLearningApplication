using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            string parolaIntrodusa = new NetworkCredential(string.Empty, txtBoxPassword.Password).Password;
            var context = new LanguageLearningApplicationDataContext();
            var user = (from u in context.Users where u.Email.Equals(txtBoxUser.Text) select u).FirstOrDefault();
            if (user != null && parolaIntrodusa == user.Password)
            {
                Home home = new Home();
                home.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Adresa de email sau parola gresite. Va rugam incercati din nou.", "Eroare de autentificare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Register reg = new Register();
            reg.Show();
            Close();
        }
    }
}
