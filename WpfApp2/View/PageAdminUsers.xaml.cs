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
    public partial class PageAdminUsers : Page
    {
        public PageAdminUsers()
        {
            InitializeComponent();
            var users = from u in AppDataContext.context.Users
                        select u ;
            DataGridUsers.ItemsSource= users;
        }

        private void DataGridUsers_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editedUser = e.Row.Item as User;
                if (editedUser != null)
                {
                    string colName = e.Column.Header.ToString();

                    var result = e.EditingElement as TextBox;

                    var user = (from c in AppDataContext.context.Users
                                  where c.UID == editedUser.UID
                                  select c).FirstOrDefault();

                   

                    if (colName == "Email")
                    {
                        var existingUser = (from c in AppDataContext.context.Users
                                            where c.Email == result.Text
                                            select c).FirstOrDefault();
                        if (existingUser != null)
                        {
                            MessageBox.Show("Deja exista un utilizator cu acest Email!", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            user.Email = result.Text;
                        }
                        
                    }
                    else if (colName == "Password")
                    {
                        user.Password = ComputeHash(result.Text);
                    }
                    else if (colName == "First Name")
                    {
                        user.FirstName = result.Text;
                    }
                    else if (colName == "Last Name")
                    {
                        user.LastName = result.Text;
                    }
                    else if (colName == "Registration Date")
                    {
                        user.RegistrationDate = DateTime.Parse(result.Text);


                    }
                    else if (colName == "Last Login Date")
                    {
                        user.LastLoginDate = DateTime.Parse(result.Text);
                        
                    }
                    else if (colName == "Role")
                    {
                        user.Role = result.Text;
                    }

                    AppDataContext.context.SubmitChanges();
                    var users = from c in AppDataContext.context.Users
                                  select c;

                    DataGridUsers.ItemsSource = users;
                }
            }
           
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            PageAddUser peq = new PageAddUser();
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

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = DataGridUsers.SelectedItem as User;
            if (selectedUser != null)
            {
                var user = (from c in AppDataContext.context.Users where c.UID == selectedUser.UID select c).FirstOrDefault();
                AppDataContext.context.Users.DeleteOnSubmit(user);
                AppDataContext.context.SubmitChanges();


                var updateusers = from c in AppDataContext.context.Users
                                    select c;

                    DataGridUsers.ItemsSource = updateusers;
            }
            else
            {
                MessageBox.Show("Selectează un user înainte de a continua.", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
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
