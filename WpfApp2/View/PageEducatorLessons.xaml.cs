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
                MessageBox.Show("Selectează o lectie înainte de a continua.", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DataGridLessons_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editedLesson = e.Row.Item as Lesson;
                
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
                }
            }

        }
    

        private void btnEditQuizee_Click(object sender, RoutedEventArgs e)
        {
            var selectedLesson = DataGridLessons.SelectedItem as Lesson;
            if (selectedLesson != null)
            {
                PageEducatorQuizee peq = new PageEducatorQuizee(this.CID, selectedLesson.LID);
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
            else
            {
                MessageBox.Show("Selectează o lectie înainte de a continua.", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void DataGridLessons_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            string colName = e.Column.Header.ToString();
            if(colName == "Content")
            {
                var editedLesson = e.Row.Item as Lesson;
                //lesson.Content = result.Text;

                DataGridCellInfo currentCell = DataGridLessons.CurrentCell;
                int rowIndex = DataGridLessons.Items.IndexOf(currentCell.Item);
                int colIndex = DataGridLessons.Columns.IndexOf(currentCell.Column);
                string currentText = DataGridLessons.Items[rowIndex].GetType().GetProperty(DataGridLessons.Columns[colIndex].SortMemberPath).GetValue(DataGridLessons.Items[rowIndex])?.ToString();
                PageEducatorModifyContent pec = new PageEducatorModifyContent(editedLesson.LID, this.CID, currentText);
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
        }
    }
}
