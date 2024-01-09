using Microsoft.Win32;
using System;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2.View
{
    /// <summary>
    /// Interaction logic for PageProfile.xaml
    /// </summary>
    public partial class PageProfile : Page
    {
        public PageProfile()
        {
            InitializeComponent();
            lblPasswd.Visibility = Visibility.Hidden;
            txtBPasswd.Visibility = Visibility.Hidden;
            btnSavePasswd.Visibility = Visibility.Hidden;

            var progress =(from p in AppDataContext.context.Progresses
                       where p.UID == UserProfile.user.UID
                       select p).FirstOrDefault();
            if(progress != null)
            {
                txtBQuizeesScore.Text = progress.QuizScores.ToString();
                txtBTSL.Text = $"{progress.TimeSpentLearning/60/60} hours, {progress.TimeSpentLearning/60%60} minutes, {progress.TimeSpentLearning%60} seconds "; 
            }
            
            txtBRole.Text = UserProfile.user.Role;
            txtBLName.Text = UserProfile.user.LastName;
            txtBFName.Text = UserProfile.user.FirstName;
            txtBEmail.Text = UserProfile.user.Email;
            
        }

        private void btnChangePasswd_Click(object sender, RoutedEventArgs e)
        {
            lblPasswd.Visibility = Visibility.Visible;
            txtBPasswd.Visibility = Visibility.Visible;
            btnSavePasswd.Visibility = Visibility.Visible;
        }
        

        private void btnSavePasswd_Click(object sender, RoutedEventArgs e)
        {
            int n = 3;
            string parola = txtBPasswd.Password;
            if(txtBPasswd.Password!= null && parola.Length>n)
            {
                string hashedPassword = ComputeHash(ConvertSecureStringToString(txtBPasswd.SecurePassword));
                var updateduser = (from u in AppDataContext.context.Users
                                   where u.UID == UserProfile.user.UID
                                   select u).FirstOrDefault();

                updateduser.Password = hashedPassword;

                AppDataContext.context.SubmitChanges();

                btnSavePasswd.Visibility = Visibility.Hidden;
                lblPasswd.Visibility = Visibility.Hidden;
                txtBPasswd.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show($"Parola nu poate fi mai mica de {n} caractere!", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void btnAddProfilePicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            

            


            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));

                var parentWindow = Window.GetWindow(this);
                var ellipse = LogicalTreeHelper.FindLogicalNode(parentWindow, "myEllipse") as Ellipse;

                if (ellipse.Fill is ImageBrush imageBrush)
                {
                    imageBrush.ImageSource = bitmapImage;
                }
                string imagePath = openFileDialog.FileName;
                byte[] imageData = File.ReadAllBytes(imagePath);
                UserProfile.user.ProfilePicture = imageData;
                AppDataContext.context.SubmitChanges();
            }
        }

        private void txtBFName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var user = (from u in AppDataContext.context.Users
                            where u.UID == UserProfile.user.UID
                            select u).FirstOrDefault();

                user.FirstName = txtBFName.Text;

                AppDataContext.context.SubmitChanges();
            }
        }

        private void txtBLName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var user = (from u in AppDataContext.context.Users
                            where u.UID == UserProfile.user.UID
                            select u).FirstOrDefault();

                user.LastName = txtBLName.Text;

                AppDataContext.context.SubmitChanges();
            }
        }

        private void txtBEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var existingMail = (from m in AppDataContext.context.Users
                                    where m.Email == txtBEmail.Text
                                    select m).FirstOrDefault();

                if (existingMail != null)
                {
                    MessageBox.Show("Nu pot exista doi utilizatori cu acelasi email!", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtBEmail.Text = UserProfile.user.Email;
                    return;
                }

                var user = (from u in AppDataContext.context.Users
                            where u.UID == UserProfile.user.UID
                            select u).FirstOrDefault();

                user.Email = txtBEmail.Text;

                AppDataContext.context.SubmitChanges();
            }
        }

        private void txtBFName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtBFName.Text= UserProfile.user.FirstName;
        }

        private void txtBLName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtBLName.Text= UserProfile.user.LastName;
        }

        private void txtBEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            txtBEmail.Text= UserProfile.user.Email;
        }
    }
}
