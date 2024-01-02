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

namespace WpfApp2.View
{
    /// <summary>
    /// Interaction logic for PageUserCourses.xaml
    /// </summary>
    
    public partial class PageUserCourses : Page
    {
        public PageUserCourses()
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

        private void btnResumeCourse_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);

            // Verifică dacă fereastra părinte este disponibilă
            if (parentWindow != null)
            {

                CoursWindow lg = new CoursWindow();
                lg.Show();
                parentWindow.Close();
            }
        }

       
    }
}
