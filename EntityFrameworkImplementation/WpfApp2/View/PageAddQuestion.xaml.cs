using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class PageAddQuestion : Page
    {
        private int CID;
        private int LID;
        private int QID;
        public PageAddQuestion(int CID,int LID, int qID)
        {
            InitializeComponent();
            this.CID = CID;
            this.LID = LID;
            QID = qID;
            txtBCorrect.Items.Add("First Answer");
            txtBCorrect.Items.Add("Second Answer");
            txtBCorrect.Items.Add("Third Answer");
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

        private void btnAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            if(txtBCorrect.Text.Length > 0 && txtBFirstAns.Text.Length>0 && txtBQuestion.Text.Length>0 &&
                txtBSecondAns.Text.Length>0 && txtBThirdAns.Text.Length>0) {

                if(txtBCorrect.Text=="First Answer" || txtBCorrect.Text == "Second Answer" || txtBCorrect.Text == "Third Answer")
                {
                    Question newQuestion = new Question
                    {
                        QID = this.QID,
                        Question1 = txtBQuestion.Text,
                        FirstAnswer = txtBFirstAns.Text,
                        SecondAnswer = txtBSecondAns.Text,
                        ThirdAnswer = txtBThirdAns.Text,
                        IsCorrect = txtBCorrect.Text
                    };

                    AppDataContext.context.Questions.Add(newQuestion);
                    AppDataContext.context.SaveChanges();

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
                    MessageBox.Show("FirstAnswer sau SecondAnswer sau ThirdAnswer!", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Completeaza toate campurile!", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
           
        }
    }
}
