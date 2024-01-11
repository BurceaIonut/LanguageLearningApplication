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
    /// Interaction logic for PageEducatorModifyContent.xaml
    /// </summary>
    public partial class PageEducatorModifyContent : Page
    {
        private int LID;
        private int CID;
        private string content;
        public PageEducatorModifyContent(int lID,int CID, string content)
        {
            InitializeComponent();
            this.LID = lID;
            this.CID = CID;
            this.content = content;
            txtBContent.Text = content;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var lesson = (from l in AppDataContext.context.Lessons
                          where l.LID== this.LID
                          select l).FirstOrDefault();

            if (lesson.Content != txtBContent.Text)
            {
                lesson.Content = txtBContent.Text;
                AppDataContext.context.SubmitChanges();
            }
            

            PageEducatorLessons pec = new PageEducatorLessons(this.CID);
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
