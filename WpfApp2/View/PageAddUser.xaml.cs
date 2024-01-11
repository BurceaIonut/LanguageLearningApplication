using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2.View
{
    public partial class PageAddUser : Page
    {
        public PageAddUser()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            PageAdminUsers peq = new PageAdminUsers();
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                var frame = LogicalTreeHelper.FindLogicalNode(parentWindow, "FrameMain") as Frame;
                if (frame != null)
                {
                    frame.Content = peq;
                }
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            if (txtBFName.Text.Length > 0 && txtBLName.Text.Length > 0 && txtBPasswd.Text.Length > 0 &&
               txtBRole.Text.Length > 0 && txtBEmail.Text.Length > 0)
            {

                if (txtBRole.Text == "Educator" || txtBRole.Text == "Member" || txtBRole.Text == "admin")
                {
                    User newUser = new User
                    {
                        FirstName = txtBFName.Text,
                        LastName = txtBLName.Text,
                        Email = txtBEmail.Text,
                        Password = ComputeHash(txtBPasswd.Text),
                        Role = txtBRole.Text,
                        LastLoginDate= DateTime.Now,
                        RegistrationDate= DateTime.Now
                    };

                    AppDataContext.context.Users.InsertOnSubmit(newUser);
                    AppDataContext.context.SubmitChanges();

                    PageAdminUsers peq = new PageAdminUsers();
                    var parentWindow = Window.GetWindow(this);
                    if (parentWindow != null)
                    {
                        var frame = LogicalTreeHelper.FindLogicalNode(parentWindow, "FrameMain") as Frame;
                        if (frame != null)
                        {
                            frame.Content = peq;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Educator sau Member sau admin!", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Completeaza toate campurile!", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
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
