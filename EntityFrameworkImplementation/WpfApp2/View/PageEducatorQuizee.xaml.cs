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
    /// Interaction logic for PageEducatorQuizee.xaml
    /// </summary>
    public partial class PageEducatorQuizee : Page
    {
        private int CID;
        private int LID;
        

        public PageEducatorQuizee(int CID, int lID)
        {
            InitializeComponent();
            this.CID = CID;
            LID = lID;
            
            var quiz = (from q in AppDataContext.context.Quizzes
                        where q.LID == LID
                        select q).ToList();

            DataGridQuizees.ItemsSource = quiz.ToList();

            if (quiz.Count() > 0) {
                int qid = quiz[0].QID;

                var questions = from q in AppDataContext.context.Questions
                                where q.QID == qid
                                select q;
                DataGridQuestionsAnswers.ItemsSource = questions.ToList();
            } 

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

        private void DataGriQuizees_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editedQuiz = e.Row.Item as Quizze;

                if (editedQuiz != null)
                {
                    string colName = e.Column.Header.ToString();
                    int quizID = editedQuiz.QID;

                    var result = e.EditingElement as TextBox;

                    var quiz = (from q in AppDataContext.context.Quizzes
                                  where q.QID == quizID
                                  select q).FirstOrDefault();
                    if (colName == "Title")
                    {
                        if (result.Text.Length > 0)
                            quiz.Title = result.Text;
                    }
                    else if (colName == "Difficulty level")
                    {
                        if (result.Text.Length > 0)
                            quiz.DifficultyLevel = result.Text;
                    }
                   
                    AppDataContext.context.SaveChanges();
                    var quizzes = from c in AppDataContext.context.Quizzes
                                  where c.LID == this.LID
                                  select c;

                        DataGridQuizees.ItemsSource = quizzes.ToList();
                }
            }
        }

        private void DataGridQuestionsAnswers_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editedQuestion = e.Row.Item as Question;

                if (editedQuestion != null)
                {
                    string colName = e.Column.Header.ToString();
                    int questionID = editedQuestion.QUID;

                    var result = e.EditingElement as TextBox;

                    var question = (from q in AppDataContext.context.Questions
                                where q.QUID == questionID
                                    select q).FirstOrDefault();
                    if (colName == "Question")
                    {
                        if(result.Text.Length > 0)
                            question.Question1 = result.Text;
                    }
                    else if (colName == "First Answer")
                    {
                        if (result.Text.Length > 0)
                            question.FirstAnswer = result.Text;
                    }
                    else if (colName == "Second Answer")
                    {
                        if (result.Text.Length > 0)
                            question.SecondAnswer = result.Text;
                    }
                    else if (colName == "Third Answer")
                    {
                        if (result.Text.Length > 0)
                            question.ThirdAnswer = result.Text;
                    }
                    else if (colName == "Correct Answer")
                    {
                        if (result.Text.Length > 0)
                            question.IsCorrect = result.Text;
                    }
                    AppDataContext.context.SaveChanges();
                    var qid = (from q in AppDataContext.context.Quizzes
                              where q.LID == this.LID
                              select q.QID).FirstOrDefault();

                    var questions = from c in AppDataContext.context.Questions
                                  where c.QID == qid
                                  select c;

                    DataGridQuestionsAnswers.ItemsSource = questions.ToList();
                }
            }
        }

        private void btnDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            var selectedQuestion = DataGridQuestionsAnswers.SelectedItem as Question;

            if (selectedQuestion != null)
            {
                var question = (from c in AppDataContext.context.Questions where c.QUID == selectedQuestion.QUID select c).FirstOrDefault();
                AppDataContext.context.Questions.Remove(question);
                AppDataContext.context.SaveChanges();

                var qid = (from q in AppDataContext.context.Quizzes
                           where q.LID == this.LID
                           select q.QID).FirstOrDefault();

                var updateQuestions = from c in AppDataContext.context.Questions
                                    where c.QID == qid 
                                    select c;

                if (updateQuestions != null)
                    DataGridQuestionsAnswers.ItemsSource = updateQuestions.ToList();


               
            }
            else
            {
                MessageBox.Show("Selectează o intrebare înainte de a continua.", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDeleteQuizee_Click(object sender, RoutedEventArgs e)
        {
            var selectedQuiz = DataGridQuizees.SelectedItem as Quizze;

            if (selectedQuiz != null)
            {
                var quiz = (from c in AppDataContext.context.Quizzes where c.QID == selectedQuiz.QID select c).FirstOrDefault();
                AppDataContext.context.Quizzes.Remove(quiz);
                AppDataContext.context.SaveChanges();

                var updateQuizees = from c in AppDataContext.context.Quizzes
                                      where c.LID == this.LID
                                      select c;

                if (updateQuizees != null)
                    DataGridQuestionsAnswers.ItemsSource = updateQuizees.ToList();

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
                MessageBox.Show("Selectează un quiz înainte de a continua.", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnAddQuizee_Click(object sender, RoutedEventArgs e)
        {
            var existingquiz = (from q in AppDataContext.context.Quizzes
                                where q.LID == this.LID
                                select q).FirstOrDefault();
            if (existingquiz != null)
            {
                MessageBox.Show("Exista deja un quiz asociat lectiei!", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PageAddQuizee paq = new PageAddQuizee(this.CID,this.LID);
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                var frame = LogicalTreeHelper.FindLogicalNode(parentWindow, "FrameMain") as Frame;
                if (frame != null)
                {
                    frame.Content = paq;
                }
            }


        }

        private void btnAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridQuizees.Items.Count > 0)
            {
                var qid = (from q in AppDataContext.context.Quizzes
                           where q.LID == this.LID
                           select q.QID).FirstOrDefault();
                PageAddQuestion peq = new PageAddQuestion(this.CID, this.LID, qid);
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
                MessageBox.Show("Prima oara trebuie introdus un Quiz!", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
