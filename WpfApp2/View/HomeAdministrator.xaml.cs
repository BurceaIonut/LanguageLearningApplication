using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for HomeAdministrator.xaml
    /// </summary>
    public partial class HomeAdministrator : Window
    {
        public HomeAdministrator()
        {
            InitializeComponent();
            FrameMain.Content = new PageAdminUsers();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginView lg = new LoginView();
            lg.Show();
            Close();
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

        private void BtnUseri_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Content = new PageAdminUsers();
        }
    }
}
