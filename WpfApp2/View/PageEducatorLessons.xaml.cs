using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for PageEducatorLessons.xaml
    /// </summary>
    public partial class PageEducatorLessons : Page
    {
        private int CID;
        public PageEducatorLessons(int CID)
        {
            InitializeComponent();
            this.CID = CID;
            var lessons = from c in AppDataContext.context.Lessons
                          where c.CID == CID
                          orderby c.OrderInCourse
                          select c;


            if (lessons != null)
                DataGridLessons.ItemsSource = lessons;
        }

        private void btnDeleteLesson_Click(object sender, RoutedEventArgs e)
        {
            var selectedLesson = DataGridLessons.SelectedItem as Lesson;

            // Verificarea dacă există un curs selectat
            if (selectedLesson != null)
            {
                var lesson = (from c in AppDataContext.context.Lessons where c.LID == selectedLesson.LID select c).FirstOrDefault();
                AppDataContext.context.Lessons.DeleteOnSubmit(lesson);
                AppDataContext.context.SubmitChanges();


                var updatelessons = from c in AppDataContext.context.Lessons
                              where c.CID == CID && c.OrderInCourse > lesson.OrderInCourse
                              orderby c.OrderInCourse
                              select c;

                foreach(Lesson l in updatelessons)
                {
                    l.OrderInCourse--;
                }
                AppDataContext.context.SubmitChanges();

                var lessons = from c in AppDataContext.context.Lessons
                              where c.CID == CID
                              orderby c.OrderInCourse
                              select c;


                if (lessons != null)
                    DataGridLessons.ItemsSource = lessons;
            }
            else
            {
                // În cazul în care nu există niciun curs selectat, poți afișa un mesaj sau să iei alte acțiuni
                MessageBox.Show("Selectează un curs înainte de a continua.", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DataGridLessons_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editedLesson = e.Row.Item as Lesson;

                // Verifică dacă cursul este nul și efectuează acțiunile necesare
                if (editedLesson != null)
                {
                    string colName = e.Column.Header.ToString();
                    int lessonID = editedLesson.LID;

                    var result = e.EditingElement as TextBox;

                    var lesson = (from c in AppDataContext.context.Lessons
                                  where c.LID == lessonID
                                  select c).FirstOrDefault();
                    if (colName == "Title")
                    {
                        lesson.Title = result.Text;
                    }
                    else if (colName == "Content")
                    {
                        lesson.Content = result.Text;
                    }
                    else if (colName == "Order in Course")
                    {
                        var lastLesson = (from l in AppDataContext.context.Lessons
                                          orderby l.OrderInCourse descending
                                          select l).FirstOrDefault();
                        if(int.Parse(result.Text) > lastLesson.OrderInCourse)
                        {
                            MessageBox.Show("Ordinea este incorecta!", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            var chancgedLesson = (from l in AppDataContext.context.Lessons
                                                  where l.OrderInCourse == int.Parse(result.Text) && l.CID == this.CID
                                                  select l).FirstOrDefault();
                            
                            var order = lesson.OrderInCourse;
                            lesson.OrderInCourse = int.Parse(result.Text);
                            chancgedLesson.OrderInCourse = order;

                           
                        }
                        
                    }
                    

                    AppDataContext.context.SubmitChanges();
                    var lessons = from c in AppDataContext.context.Lessons
                                  where c.CID == this.CID
                                  orderby c.OrderInCourse
                                  select c;


                    if (lessons != null)
                        DataGridLessons.ItemsSource = lessons;
                    // Aici poți efectua acțiunile necesare pentru salvarea modificărilor
                    // Poți utiliza valorile noi din e.EditingElement pentru a actualiza obiectul Course.
                }
            }

        }
    

        private void btnEditQuizee_Click(object sender, RoutedEventArgs e)
        {
            var selectedLesson = DataGridLessons.SelectedItem as Lesson;
            PageEducatorQuizee peq = new PageEducatorQuizee(this.CID,selectedLesson.LID);
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                var frame = LogicalTreeHelper.FindLogicalNode(parentWindow, "FrameMain") as Frame;
                if (frame != null)
                {
                    frame.Content = peq;
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            PageEducatorCourses pec = new PageEducatorCourses();
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                var frame = LogicalTreeHelper.FindLogicalNode(parentWindow, "FrameMain") as Frame;
                if (frame != null)
                {
                    frame.Content = pec;
                }
            }
        }

        private void btnAddLesson_Click(object sender, RoutedEventArgs e)
        {
           

                PageAddLesson pal = new PageAddLesson(this.CID);
                var parentWindow = Window.GetWindow(this);
                if (parentWindow != null)
                {
                    var frame = LogicalTreeHelper.FindLogicalNode(parentWindow, "FrameMain") as Frame;
                    if (frame != null)
                    {
                        frame.Content = pal;
                    }
                }
           
        }
    }
}
