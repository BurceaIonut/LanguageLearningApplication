using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for PageEducatorCourses.xaml
    /// </summary>
    public partial class PageEducatorCourses : Page
    {
        public PageEducatorCourses()
        {
            InitializeComponent();
           
            var courses = from c in AppDataContext.context.Courses
                          where c.CreatedBy == UserProfile.user.UID
                          select c;


            if (courses != null)
                DataGridCourses.ItemsSource = courses;
        }

        private void btnDeleteCourse_Click(object sender, RoutedEventArgs e)
        {
            var selectedCourse = DataGridCourses.SelectedItem as Course;

            // Verificarea dacă există un curs selectat
            if (selectedCourse != null)
            {
                var course = (from c in AppDataContext.context.Courses where c.CID == selectedCourse.CID select c).FirstOrDefault(); 
                AppDataContext.context.Courses.DeleteOnSubmit(course);
                AppDataContext.context.SubmitChanges();
                var courses = from c in AppDataContext.context.Courses
                              where c.CreatedBy == UserProfile.user.UID
                              select c;


                if (courses != null)
                    DataGridCourses.ItemsSource = courses;
            }
            else
            {
                // În cazul în care nu există niciun curs selectat, poți afișa un mesaj sau să iei alte acțiuni
                MessageBox.Show("Selectează un curs înainte de a continua.", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnEditCourse_Click(object sender, RoutedEventArgs e)
        {
            var selectedCourse = DataGridCourses.SelectedItem as Course;

            // Verificarea dacă există un curs selectat
            if (selectedCourse != null)
            {
                PageEducatorLessons pel = new PageEducatorLessons(selectedCourse.CID);
                var parentWindow = Window.GetWindow(this);
                if (parentWindow != null)
                {
                    var frame = LogicalTreeHelper.FindLogicalNode(parentWindow, "FrameMain") as Frame;
                    if (frame != null)
                    {
                        frame.Content = pel;
                    }
                }
            }
            else
            {
                // În cazul în care nu există niciun curs selectat, poți afișa un mesaj sau să iei alte acțiuni
                MessageBox.Show("Selectează un curs înainte de a continua.", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DataGridCourses_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
          
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editedCourse = e.Row.Item as Course;

                // Verifică dacă cursul este nul și efectuează acțiunile necesare
                if (editedCourse != null)
                {
                    string colName = e.Column.Header.ToString();
                    int courseID = editedCourse.CID;

                    var result = e.EditingElement as TextBox;

                    var course = (from c in AppDataContext.context.Courses
                                  where c.CID == courseID
                                  select c).FirstOrDefault();
                    if (colName == "Name")
                    {
                        course.Name = result.Text;
                    }
                    else if (colName == "Language")
                    {
                        course.Language = result.Text;
                    }
                    else if (colName == "Description")
                    {
                        course.Description = result.Text;
                    }
                    else if (colName == "Difficulty")
                    {
                        course.DifficultyLevel = result.Text;
                    }

                    AppDataContext.context.SubmitChanges();
                    // Aici poți efectua acțiunile necesare pentru salvarea modificărilor
                    // Poți utiliza valorile noi din e.EditingElement pentru a actualiza obiectul Course.
                }
            }
            
        }
    }
}
