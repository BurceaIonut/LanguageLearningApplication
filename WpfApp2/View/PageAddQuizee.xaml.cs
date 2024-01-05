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
    /// Interaction logic for PageAddQuizee.xaml
    /// </summary>
    public partial class PageAddQuizee : Page
    {
        private int LID;
        private int CID;

        public PageAddQuizee(int CID,int LID)
        {
            InitializeComponent();
            this.LID = LID;
            this.CID = CID;
        }

        private void btnAddQuiz_Click(object sender, RoutedEventArgs e)
        {
            if(txtBDifficultyLevel.Text.Length>0 && txtBTitle.Text.Length > 0)
            {
                Quizze newQuiz = new Quizze
                {
                    LID = this.LID,
                    Title = txtBTitle.Text,
                    DifficultyLevel = txtBDifficultyLevel.Text

                };

                AppDataContext.context.Quizzes.InsertOnSubmit(newQuiz);
                AppDataContext.context.SubmitChanges();


                PageEducatorQuizee peq = new PageEducatorQuizee(this.CID, this.LID);
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
                MessageBox.Show("Completeaza toate campurile!", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            PageEducatorQuizee peq = new PageEducatorQuizee(this.CID, this.LID);
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
    }
}
