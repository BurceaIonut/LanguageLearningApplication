using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Security;

namespace WpfApp2.View
{
    /// <summary>
    /// Interaction logic for RecoverPasswordView.xaml
    /// </summary>
    public partial class RecoverPasswordView : Window
    {
        public RecoverPasswordView()
        {
            InitializeComponent();
            txtBoxToken.Visibility = Visibility.Hidden;
            txtBlock.Visibility = Visibility.Hidden;
            passwordBoxChangePasswd.Visibility = Visibility.Hidden;
            txtBlock_NewPasswd.Visibility = Visibility.Hidden;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


        private void btnRecover_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxEmail_Password.Text.Length == 0)
            {
                MessageBox.Show("Completati adresa de email!", "Eroare de procesare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if(btnRecover.Content.ToString() == "Recover password")
            {
                var user = (from u in AppDataContext.context.Users where u.Email.Equals(txtBoxEmail_Password.Text) select u).FirstOrDefault();
                if (user == null)
                {
                    MessageBox.Show("Adresa de email nu se afla in baza de date!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                string resetToken = GenerateResetToken();
                var token = new SecurityToken
                {
                    TokenString = resetToken,
                    Expired = 0
                };
                AppDataContext.context.SecurityTokens.InsertOnSubmit(token);
                AppDataContext.context.SubmitChanges();
                try
                {
                    // Step 3: Send Password Reset Email
                    MailMessage mailMessage = new MailMessage("llaengineers1@gmail.com", txtBoxEmail_Password.Text);
                    mailMessage.Subject = "Password Reset";
                    mailMessage.Body = $"This is the unique token generated for you: {resetToken}";

                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential("llaengineers1@gmail.com", "jben frwe bplt tdug");
                    smtpClient.EnableSsl = true;

                    smtpClient.Send(mailMessage);
                    MessageBox.Show($"Tokenul de resetare a fost trimis pe adresa de email specificata!", "Trimis", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtBlock.Visibility = Visibility.Visible;
                    txtBoxToken.Visibility = Visibility.Visible;
                    btnRecover.Content = "Change password";
                    passwordBoxChangePasswd.Visibility = Visibility.Visible;
                    txtBlock_NewPasswd.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error sending password reset email: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if(btnRecover.Content.ToString() == "Change password")
            {
                if(txtBoxToken.Text.Length > 0)
                {
                    var uniqueToken = (from t in AppDataContext.context.SecurityTokens where t.TokenString.Equals(txtBoxToken.Text) && t.Expired != 1 select t).FirstOrDefault();
                    if(uniqueToken != null)
                    {
                        SecureString ss = passwordBoxChangePasswd.SecurePassword;
                        string sspasswd = new NetworkCredential(string.Empty, ss).Password;
                        if(sspasswd.Length != 0)
                        {
                            var user = (from u in AppDataContext.context.Users where u.Email.Equals(txtBoxEmail_Password.Text) select u).FirstOrDefault();
                            user.Password = ComputeHash(ConvertSecureStringToString(passwordBoxChangePasswd.SecurePassword));
                            AppDataContext.context.SubmitChanges();
                            MessageBox.Show($"Parola a fost schimbata cu succes!", "Password reset", MessageBoxButton.OK, MessageBoxImage.Information);
                            uniqueToken.Expired = 1;
                            AppDataContext.context.SubmitChanges();
                            LoginView lg = new LoginView();
                            lg.Show();
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Parola nu poate fi nula!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Token incorect!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Va rugam introduceti tokenul unic generat primit pe adresa de email!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            LoginView lg = new LoginView();
            lg.Show();
            Close();
        }
        private string GenerateResetToken()
        {
            // Generate a unique token (you can use a library or a secure method)
            byte[] randomBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
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
