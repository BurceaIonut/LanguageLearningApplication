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
            //var coursesData = from Course in AppDataContext.context.Courses
            //                  select new
            //                  {
            //                      CourseID = Course.CID,
            //                      CourseName = Course.Name,
            //                      CourseDescription = Course.Description
            //                  };
            //var coursesNotStarted = from course in AppDataContext.context.Courses
            //                        where !AppDataContext.context.StartedCourses
            //                                .Any(startedCourse => startedCourse.CID == course.CID && startedCourse.UID == UserProfile.user.UID)
            //                        select course;
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
            //TODO copiaza de la pageusercourses
            var selectedCourse = DataGridCourses.SelectedItem as dynamic;

            // Verificarea dacă există un curs selectat
            if (selectedCourse != null)
            {
                // Obținerea ID-ului cursului selectat
                int courseId = selectedCourse.CourseID;

                // Crearea unei noi instanțe a ferestrei CoursWindow și trecerea ID-ului cursului
                CoursWindow lg = new CoursWindow(UserProfile.user.FirstName, UserProfile.user.LastName, courseId);

                // Afișarea ferestrei noi
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
                
                // Închiderea ferestrei curente (a părintelui)
                Window.GetWindow(this)?.Close();
            }
            else
            {
                // În cazul în care nu există niciun curs selectat, poți afișa un mesaj sau să iei alte acțiuni
                MessageBox.Show("Selectează un curs înainte de a continua.", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

       
    }
}
