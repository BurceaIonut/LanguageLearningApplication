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
    /// Interaction logic for PageAddCourses.xaml
    /// </summary>
    public partial class PageAddCourses : Page
    {
        public PageAddCourses()
        {
            InitializeComponent();
        }

        private void btnAddCours_Click(object sender, RoutedEventArgs e)
        {
            if (txtBDescription.Text.Length>0 && txtBDifficulty.Text.Length>0 && txtBLanguage.Text.Length>0 && txtBTitle.Text.Length>0)
            {
                Course newCourse = new Course
                {
                    CreatedBy = UserProfile.user.UID,
                    Name = txtBTitle.Text,
                    Description = txtBDescription.Text,
                    Language = txtBLanguage.Text,
                    DifficultyLevel = txtBDifficulty.Text
                };

                AppDataContext.context.Courses.InsertOnSubmit(newCourse);
                AppDataContext.context.SubmitChanges();
            }
            else
            {
                MessageBox.Show("Toate campurile trebuie completate", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
    }
}
