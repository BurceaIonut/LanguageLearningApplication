using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public partial class HomeEducator : Window
    {
        public HomeEducator()
        {
            InitializeComponent();
            txtUserName.Text = UserProfile.user.FirstName + " " + UserProfile.user.LastName;
            FrameMain.Content = new PageEducatorCourses();
            if (UserProfile.user.ProfilePicture != null)
            {
                BitmapImage bitmapImage = ConvertToBitmapImage(UserProfile.user.ProfilePicture.ToArray());
                if (myEllipse.Fill is ImageBrush imageBrush)
                {
                    imageBrush.ImageSource = bitmapImage;
                }
            }
        }

        private void BtnEducatorCourses_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Content = new PageEducatorCourses();
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

        private void BtnAddCourses_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Content =new PageAddCourses();
        }

        private void BtnShowProfile_Click(object sender, RoutedEventArgs e)
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
}
