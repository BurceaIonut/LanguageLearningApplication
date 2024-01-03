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
        private string firstName;
        private string lastName;

        public PageUserCourses(string firstName, string lastName)
        {
            InitializeComponent();

            this.firstName = firstName;
            this.lastName = lastName;
            //var coursesData = from Course in AppDataContext.context.Courses
            //                  select new
            //                  {
            //                      CourseID = Course.CID,
            //                      CourseName = Course.Name,
            //                      CourseDescription = Course.Description
            //                  };
            var startedCourses = from s in AppDataContext.context.StartedCourses
                                 join u in AppDataContext.context.Users on s.UID equals u.UID
                                 join c in AppDataContext.context.Courses on s.CID equals c.CID
                                 where u.UID == UserProfile.user.UID
                                 select new
                                 {
                                     CourseID = c.CID,
                                     Name = c.Name,
                                     Language= c.Language,
                                     Description = c.Description,
                                     Difficulry = c.DifficultyLevel
                                 };
            DataGridCourses.ItemsSource = startedCourses; 
        }

        private void btnResumeCourse_Click(object sender, RoutedEventArgs e)
        {
            //Window parentWindow = Window.GetWindow(this);

            // Verifică dacă fereastra părinte este disponibilă
            //if (parentWindow != null)
            //{

            //    CoursWindow lg = new CoursWindow(DataGridCourses.Items[0].);
            //    lg.Show();
            //    parentWindow.Close();
            //}

            var selectedCourse = DataGridCourses.SelectedItem as dynamic;

            // Verificarea dacă există un curs selectat
            if (selectedCourse != null)
            {
                // Obținerea ID-ului cursului selectat
                int courseId = selectedCourse.CourseID;

                // Crearea unei noi instanțe a ferestrei CoursWindow și trecerea ID-ului cursului
                CoursWindow lg = new CoursWindow(this.firstName, this.lastName, courseId);

                // Afișarea ferestrei noi
                lg.Show();

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


