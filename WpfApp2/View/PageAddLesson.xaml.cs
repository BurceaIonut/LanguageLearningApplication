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
    public partial class PageAddLesson : Page
    {
        private int CID;
        public PageAddLesson(int CID)
        {
            InitializeComponent();
            this.CID = CID;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            PageEducatorLessons pel = new PageEducatorLessons(this.CID);
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

        private void btnAddLesson_Click(object sender, RoutedEventArgs e)
        {
            if(txtBContent.Text.Length > 0 && txtBTitle.Text.Length>0) {
                var lessons = (from l in AppDataContext.context.Lessons
                               where l.CID == this.CID && l.Title == txtBTitle.Text
                               select l).FirstOrDefault();
                if (lessons != null)
                {
                    MessageBox.Show("Deja exista o lectie cu acest titlu!", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int order = (from l in AppDataContext.context.Lessons
                             where l.CID == this.CID
                             orderby l.OrderInCourse descending
                             select l.OrderInCourse).FirstOrDefault();
                Lesson newLesson = new Lesson {
                    CID= this.CID,
                    Title= txtBTitle.Text,
                    Content= txtBContent.Text,
                    OrderInCourse= order+1
                };
                AppDataContext.context.Lessons.InsertOnSubmit(newLesson);
                AppDataContext.context.SubmitChanges();

                PageEducatorLessons pel = new PageEducatorLessons(this.CID);
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
                MessageBox.Show("Toate campurile trebuie completate", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

       
    }
}
