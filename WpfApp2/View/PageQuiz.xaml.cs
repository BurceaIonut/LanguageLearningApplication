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
    /// Interaction logic for PageQuiz.xaml
    /// </summary>
    public partial class PageQuiz : Page
    {
        private int QID;
        private bool isLastQuiz;
        private IEnumerable<Question> questions = Enumerable.Empty<Question>();
       
        
        public PageQuiz(int QID,bool isLastQuiz)
        {
            InitializeComponent();
            this.QID= QID;
            this.isLastQuiz= isLastQuiz;
            questions = (from q in AppDataContext.context.Questions
                         where q.QID == this.QID
                         select q);
            
            foreach(Question q in questions)
            {
                GroupBox groupBox = new GroupBox();
                groupBox.Header= q.Question1;

                StackPanel innerStackPanel = new StackPanel();
                groupBox.Content = innerStackPanel;

                RadioButton radioButton = new RadioButton();
                radioButton.Content = q.FirstAnswer;

                radioButton.GroupName = $"Group_{q.QUID}";

                innerStackPanel.Children.Add(radioButton);
                RadioButton radioButton1 = new RadioButton();
                radioButton1.Content = q.SecondAnswer;

                radioButton1.GroupName = $"Group_{q.QUID}";

                innerStackPanel.Children.Add(radioButton1);
                RadioButton radioButton2 = new RadioButton();
                radioButton2.Content = q.ThirdAnswer;

                radioButton2.GroupName = $"Group_{q.QUID}";

                innerStackPanel.Children.Add(radioButton2);
                
                innerStackPanel.Children.Add(new TextBlock { Text = " ", Height = 10 });
                stkPnl1.Children.Add(groupBox);
                
               
            }



        }

        private void btnSubmitAns_Click(object sender, RoutedEventArgs e)
        {
            List<string> selectedAnswers = GetSelectedAnswers();

            if (!AreAllQuestionsAnswered())
            {
                MessageBox.Show("Te rugăm să răspunzi la toate întrebările înainte de a continua.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int nr = 0;
            
            for(int i = 0; i < questions.Count(); i++)
            {
                if (selectedAnswers[i] == questions.ElementAt(i).FirstAnswer && questions.ElementAt(i).IsCorrect == "FirstAnswer") nr++;
                if (selectedAnswers[i] == questions.ElementAt(i).SecondAnswer && questions.ElementAt(i).IsCorrect == "SecondAnswer") nr++;
                if (selectedAnswers[i] == questions.ElementAt(i).ThirdAnswer && questions.ElementAt(i).IsCorrect == "ThirdAnswer") nr++;
            }

            
            MessageBox.Show($"Ai raspuns corect la {nr} intrebari din {questions.Count()}!", "Felicitari", MessageBoxButton.OK, MessageBoxImage.Information);

            if (nr >= 3/4*questions.Count())
            {
                var completed = new CompletedQuizze
                {
                    UID = UserProfile.user.UID,
                    QID = this.QID
                };
                AppDataContext.context.CompletedQuizzes.InsertOnSubmit(completed);
                AppDataContext.context.SubmitChanges();
                
                if (this.isLastQuiz)
                {
                    var lid = (from q in AppDataContext.context.Quizzes
                               where q.QID == this.QID
                               select q.LID).FirstOrDefault();

                    int cid = (from q in AppDataContext.context.Quizzes
                               join l in AppDataContext.context.Lessons on q.LID equals l.LID 
                               join c in AppDataContext.context.Courses on l.CID equals c.CID
                               where q.LID == lid
                               select c.CID).FirstOrDefault();
                    var completedCourse = new CompletedCourse
                    {
                        UID = UserProfile.user.UID,
                        CID = cid,
                        DateCompleted = DateTime.Now
                    };

                    AppDataContext.context.CompletedCourses.InsertOnSubmit(completedCourse);
                    AppDataContext.context.SubmitChanges();
                    var StartedCourse = (from s in AppDataContext.context.StartedCourses
                               join c in AppDataContext.context.Courses on s.CID equals c.CID
                               join u in AppDataContext.context.Users on s.UID equals u.UID
                               where c.CID == cid && u.UID == UserProfile.user.UID
                               select s).FirstOrDefault();
                    
                    AppDataContext.context.StartedCourses.DeleteOnSubmit(StartedCourse);
                    AppDataContext.context.SubmitChanges();

                    var progress = (from p in AppDataContext.context.Progresses
                                    where p.UID == UserProfile.user.UID
                                    select p).FirstOrDefault();
                    if(progress == null)
                    {
                        Progress progres = new Progress {
                            UID = UserProfile.user.UID,
                            CompletedLessons = 1,
                            DailyStreak = 0,
                            QuizScores = nr,
                            TimeSpentLearning = 0

                        };
                        AppDataContext.context.Progresses.InsertOnSubmit(progres);
                        AppDataContext.context.SubmitChanges();
                    }
                    else
                    {
                        progress.QuizScores += nr;
                        progress.CompletedLessons++;
                        AppDataContext.context.SubmitChanges();
                    }

                }

                NavigationService.Navigate(null);
            }

        }
        
        private List<string> GetSelectedAnswers()
        {
            List<string> selectedAnswers = new List<string>();

            foreach (var control in stkPnl1.Children)
            {
                if (control is GroupBox groupBox)
                {
                    foreach (var innerControl in (groupBox.Content as StackPanel).Children)
                    {
                        if (innerControl is RadioButton radioButton && radioButton.IsChecked == true)
                        {
                            selectedAnswers.Add(radioButton.Content.ToString());
                        }
                    }
                }
            }

            return selectedAnswers;
        }
        private bool AreAllQuestionsAnswered()
        {
            foreach (var control in stkPnl1.Children)
            {
                if (control is GroupBox groupBox)
                {
                    bool isAnyAnswerSelected = false;

                    foreach (var innerControl in (groupBox.Content as StackPanel).Children)
                    {
                        if (innerControl is RadioButton radioButton && radioButton.IsChecked == true)
                        {
                            isAnyAnswerSelected = true;
                            break;  
                        }
                    }

                    if (!isAnyAnswerSelected)
                    {
                        return false;  
                    }
                }
            }

            return true; 
        }

        
    }
}
