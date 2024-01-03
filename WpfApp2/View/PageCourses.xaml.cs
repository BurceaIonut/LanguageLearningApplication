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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.View;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for PageCourses.xaml
    /// </summary>
    public partial class PageCourses : Page
    {
        public PageCourses()
        {
            InitializeComponent();
            var coursesData = from Course in AppDataContext.context.Courses
                              select new
                              {
                                  CourseID = Course.CID,
                                  CourseName = Course.Name,
                                  CourseDescription = Course.Description
                              };
            DataGridCourses.ItemsSource = coursesData;
        }

        private void btnStartCourse_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }
    }
}
