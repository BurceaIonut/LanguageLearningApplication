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
    public partial class PageAddCourses : Page
    {
        public PageAddCourses()
        {
            InitializeComponent();
            txtBDifficulty.Items.Add("Easy");
            txtBDifficulty.Items.Add("Medium");
            txtBDifficulty.Items.Add("Hard");
            txtBDifficulty.Items.Add("Insane");
        }

        private void btnAddCours_Click(object sender, RoutedEventArgs e)
        {
            if (txtBDescription.Text.Length>0 && txtBDifficulty.Text.Length>0 && txtBLanguage.Text.Length>0 && txtBTitle.Text.Length>0)
            {
                Cours newCourse = new Cours
                {
                    CreatedBy = UserProfile.user.UID,
                    Name = txtBTitle.Text,
                    Description = txtBDescription.Text,
                    Language = txtBLanguage.Text,
                    DifficultyLevel = txtBDifficulty.Text
                };
                
                AppDataContext.context.Courses.Add(newCourse);
                AppDataContext.context.SaveChanges();
                PageEducatorCourses pel = new PageEducatorCourses();
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
