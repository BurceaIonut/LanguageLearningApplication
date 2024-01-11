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
using System.Windows.Shapes;

namespace WpfApp2.View
{
    public partial class CoursWindow : Window
    {
        private string firstName;
        private string lastName;
        private DateTime timeSpent;
        private IEnumerable<Lesson> lessons;
        IEnumerable<Quizze> quizzes= Enumerable.Empty<Quizze>();
        public CoursWindow(string firstName,string lastName,int ID)
        {
            InitializeComponent();
            this.firstName = firstName;
            this.lastName = lastName;
            this.timeSpent = DateTime.Now;
            lessons = (from l in AppDataContext.context.Lessons
                          where l.CID == ID
                           orderby l.OrderInCourse 
                           select l) ;
            lblCoursNAme.Text = (from c in AppDataContext.context.Courses
                                 where c.CID == ID
                                 select c.Language).FirstOrDefault();
            long nrLessons = 0;
            foreach (var l in lessons)
            {
                
                Button button = new Button();
                button.Name = $"btn{ l.OrderInCourse.ToString()}";
                button.Style = (Style)FindResource("menuButton");
                button.Content = l.Title;
                button.Click += Button_Click;
                stkPnl.Children.Add(button);
                nrLessons++;
                Quizze quiz = (from q in AppDataContext.context.Quizzes
                               where q.LID == l.LID
                               select q).FirstOrDefault();
                if (quiz != null)
                {

                    quizzes.Append(quiz);
                    Button buttonQuiz = new Button();
                    if (nrLessons == lessons.LongCount())
                    {
                        buttonQuiz.Name = $"btnLastQuiz{quiz.QID.ToString()}";
                    }
                    else
                    {
                        buttonQuiz.Name = $"btnQuiz{quiz.QID.ToString()}";
                    }
                    buttonQuiz.Margin = new Thickness(50, 0, 0, 0);
                    buttonQuiz.Style = (Style)FindResource("menuButton");
                    buttonQuiz.Content = quiz.Title;
                    buttonQuiz.Click += Button_Click;
                    stkPnl.Children.Add(buttonQuiz);
                }

            }
        }
        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            if (clickedButton.Name.Contains("Quiz"))
            {
                int order;
                if (clickedButton.Name.Contains("Last"))
                     order = int.Parse(clickedButton.Name.Substring(11));
                else
                     order = int.Parse(clickedButton.Name.Substring(7));
                var alreadyCompleted = (from cq in AppDataContext.context.CompletedQuizzes
                                        where cq.QID == order && cq.UID == UserProfile.user.UID
                                        select cq).FirstOrDefault() ;
                if (alreadyCompleted != null)
                {
                    MessageBox.Show("Deja ai completat acest quiz!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if(clickedButton.Name.Contains("Last"))
                        FrameMain.Content = new PageQuiz(order,true);
                    else
                        FrameMain.Content = new PageQuiz(order, false);
                }
            }
            else
            {
                int orderInCourse = int.Parse(clickedButton.Name.Substring(3));
                FrameMain.Content = new PageLesson(lessons.ElementAt(orderInCourse - 1).Content);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime actualLearningTime = DateTime.Now;
            TimeSpan difference = actualLearningTime - this.timeSpent;
            var updateProgress = (from p in AppDataContext.context.Progresses
                                  where p.UID == UserProfile.user.UID
                                  select p).FirstOrDefault();
            int hours = difference.Hours;
            int minutes = difference.Minutes;
            int seconds = difference.Seconds;
            int total_time = hours / 60 / 60 + minutes / 60 + seconds;
            if (updateProgress != null)
            {
                updateProgress.TimeSpentLearning += total_time;
                AppDataContext.context.SubmitChanges();
            }
            else
            {
                Progress newProgress = new Progress
                {
                    UID = UserProfile.user.UID,
                    CompletedLessons = 0,
                    DailyStreak = 0,
                    QuizScores = 0,
                    TimeSpentLearning = total_time
                };
                AppDataContext.context.Progresses.InsertOnSubmit(newProgress);
                AppDataContext.context.SubmitChanges();
            }

            Home h = new Home(this.firstName,this.lastName);
            h.Show();
            Close();
        }
    }
}
