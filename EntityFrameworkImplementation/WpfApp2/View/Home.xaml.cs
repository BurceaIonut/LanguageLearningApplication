﻿using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2.View
{
    public partial class Home : Window
    {
        private string firstName; 
        private string lastName;
        public Home(string firstName, string lastName)
        {
            InitializeComponent();
            this.firstName = firstName;
            this.lastName = lastName;
            txtUserName.Text = firstName + " " + lastName;
            FrameMain.Content = new PageUserCourses(this.firstName,this.lastName);
            if (UserProfile.user.ProfilePicture != null)
            {
                BitmapImage bitmapImage = ConvertToBitmapImage(UserProfile.user.ProfilePicture.ToArray());
                if (myEllipse.Fill is ImageBrush imageBrush)
                {
                    imageBrush.ImageSource = bitmapImage;
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginView lg = new LoginView();
            
            lg.Show();
            Close();
        }

        private void BtnShowCourses_Click(object sender, RoutedEventArgs e)
        {
            
            FrameMain.Content= new PageCourses();
        }

        private void BtnUser_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Content = new PageUserCourses(this.firstName, this.lastName);
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Content = new PageProfile();
        }
        private BitmapImage ConvertToBitmapImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }

            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memoryStream = new MemoryStream(imageData))
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }
    }

    public class Member
    {
        public string Character { get; set; }
        public Brush BgColor { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
