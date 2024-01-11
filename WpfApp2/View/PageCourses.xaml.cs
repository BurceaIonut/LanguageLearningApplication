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
    public partial class PageCourses : Page
    {
        public PageCourses()
        {
            InitializeComponent();
            var coursesNotStarted = AppDataContext.context.Courses
                    .Where(course =>
                        !AppDataContext.context.StartedCourses.Any(startedCourse =>
                            startedCourse.CID == course.CID && startedCourse.UID == UserProfile.user.UID))
                            .Select(course => new
                            {
                                CourseID = course.CID,
                               Name= course.Name,
                               DifficultyLanguage= course.DifficultyLevel,
                               Language = course.Language,
                               Description = course.Description
                            });
            DataGridCourses.ItemsSource = coursesNotStarted;
        }

        private void btnStartCourse_Click(object sender, RoutedEventArgs e)
        {
            var selectedCourse = DataGridCourses.SelectedItem as dynamic;

            if (selectedCourse != null)
            {

                int courseId = selectedCourse.CourseID;


                CoursWindow lg = new CoursWindow(UserProfile.user.FirstName, UserProfile.user.LastName, courseId);


                lg.Show();

                var completedCourse = (from c in AppDataContext.context.CompletedCourses
                                       where c.UID == UserProfile.user.UID && c.CID == courseId
                                       select c);
                if(completedCourse.LongCount() == 0 )
                {
                    StartedCourse startedCourse = new StartedCourse
                    {
                        CID = courseId,
                        UID = UserProfile.user.UID
                    };
                    AppDataContext.context.StartedCourses.InsertOnSubmit(startedCourse);
                    AppDataContext.context.SubmitChanges();
                }

                Window.GetWindow(this)?.Close();
            }
            else
            {
                MessageBox.Show("Selectează un curs înainte de a continua.", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

       
    }
}
