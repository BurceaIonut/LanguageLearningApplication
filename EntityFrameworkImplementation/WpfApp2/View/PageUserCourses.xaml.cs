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
                                     Difficulty = c.DifficultyLevel
                                 };
            
                DataGridCourses.ItemsSource = startedCourses.ToList();
            
        }

        private void btnResumeCourse_Click(object sender, RoutedEventArgs e)
        {
            
            var selectedCourse = DataGridCourses.SelectedItem as dynamic;

            if (selectedCourse != null)
            {
                int courseId = selectedCourse.CourseID;

                CoursWindow lg = new CoursWindow(this.firstName, this.lastName, courseId);

                lg.Show();
                Window.GetWindow(this)?.Close();
            }
            else
            {
                MessageBox.Show("Selectează un curs înainte de a continua.", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

       
    }


