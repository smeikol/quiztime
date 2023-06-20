using System;
using Microsoft.Win32;
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
using System.Windows.Forms;
using System.Drawing;
using quiztime.classes;
using MySqlX.XDevAPI.Relational;
using System.Reflection;
using WpfAnimatedGif;
using System.IO;

namespace quiztime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Microsoft.Win32.OpenFileDialog file = new Microsoft.Win32.OpenFileDialog();
        List<string[]> quiz = new List<string[]>();
        string destinationPath = "\\images\\shutterstock_1109972750-916x458.jpg";
        int a = 0;
        int b = 0;
        int c = 0;
        int d = 0;
        int counter;
        int maxcount;
        Boolean editmode;
        int editid;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int screenCount = Screen.AllScreens.Length;
            display win2 = new display();
            if (screenCount > 1)
            {
                var secondscreen = Screen.AllScreens[1];
                win2.WindowStartupLocation = WindowStartupLocation.Manual;
                var workingArea = secondscreen.WorkingArea;
                win2.Left = workingArea.Left / 1.25;
                win2.Top = workingArea.Top;
                // win2.Width = workingArea.Width / 1.25;
                // win2.Height = workingArea.Height;
            }
            win2.Show();
            controls win3 = new controls();
            win3.Topmost = true;
            win3.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            menu.Visibility = Visibility.Collapsed;
            quizmaker.Visibility = Visibility.Visible;
            data md = new data();
            var myquiz = md.test();
            CQuiz quiz = new CQuiz();
            quiztable.ItemsSource = myquiz;
        }

        private void newquiz_Click(object sender, RoutedEventArgs e)
        {
            quizmaker.Visibility = Visibility.Collapsed;
            quizform.Visibility = Visibility.Visible;
            editmode = false;

        }

        private void quizimage1_Click(object sender, RoutedEventArgs e)
        {
            file.Filter = "Image files|*.bmp;*.jpg;*.jepg;*.gif;*.png";
            file.FilterIndex = 1;
            if (file.ShowDialog() == true)
            {
                destinationPath = System.IO.Path.Combine(@"C:\Users\michael\source\repos\Level_8\quiztime\quiztime\images", System.IO.Path.GetFileName(file.FileName));
                Console.WriteLine(file.FileName);
                if (!File.Exists(destinationPath))
                {
                    File.Copy(file.FileName, System.IO.Path.Combine(@"C:\Users\michael\source\repos\Level_8\quiztime\quiztime\images", System.IO.Path.GetFileName(file.FileName)), true);
                }
                Console.WriteLine(destinationPath);
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(destinationPath, UriKind.RelativeOrAbsolute);
                image.EndInit();
                ImageBehavior.SetAnimatedSource(testimg, image);
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            quizform.Visibility = Visibility.Collapsed;
            vraagform.Visibility = Visibility.Visible;
            string[] quizdata = new string[2];
            quizdata[0] = quizname1.Text;
            quizdata[1] = destinationPath;

            counter = 1;
            if (editmode)
            {
                maxcount = quiz.Count;
                quiz[0] = quizdata;
                question.Text = quiz[counter][0];
                destinationPath = quiz[counter][1];
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(destinationPath, UriKind.RelativeOrAbsolute);
                image.EndInit();
                ImageBehavior.SetAnimatedSource(ques_image, image);
                timer.Text = quiz[counter][2];
                answer_a.Text = quiz[counter][3];
                if (quiz[counter][7] == "1")
                {
                    good_a.IsChecked = true;
                }
                else
                {
                    good_a.IsChecked = false;
                }
                answer_b.Text = quiz[counter][4];
                if (quiz[counter][8] == "1")
                {
                    good_b.IsChecked = true;
                }
                else
                {
                    good_b.IsChecked = false;
                }
                answer_c.Text = quiz[counter][5];
                if (quiz[counter][9] == "1")
                {
                    good_c.IsChecked = true;
                }
                else
                {
                    good_c.IsChecked = false;
                }
                answer_d.Text = quiz[counter][6];
                if (quiz[counter][10] == "1")
                {
                    good_d.IsChecked = true;
                }
                else
                {
                    good_d.IsChecked = false;
                }
            }
            else
            {
                maxcount = 1;
                quiz.Add(quizdata);
            }
        }
        private void Nextquestion_Click(object sender, RoutedEventArgs e)
        {
            string[] questiondata = new string[11];
            questiondata[0] = question.Text;
            questiondata[1] = destinationPath;
            questiondata[2] = timer.Text;
            questiondata[3] = answer_a.Text;
            questiondata[7] = Convert.ToString(a);
            questiondata[4] = answer_b.Text;
            questiondata[8] = Convert.ToString(b);
            questiondata[5] = answer_c.Text;
            questiondata[9] = Convert.ToString(c);
            questiondata[6] = answer_d.Text;
            questiondata[10] = Convert.ToString(d);


            if (maxcount == counter || maxcount == counter + 1)
            {
                quiz.Add(questiondata);
                question.Text = "vraag";
                destinationPath = "\\images\\shutterstock_1109972750-916x458.jpg";
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(destinationPath, UriKind.RelativeOrAbsolute);
                image.EndInit();
                ImageBehavior.SetAnimatedSource(ques_image, image);
                timer.Text = "timer";
                answer_a.Text = "a";
                good_a.IsChecked = false;
                answer_b.Text = "b";
                good_b.IsChecked = false;
                answer_c.Text = "c";
                good_c.IsChecked = false;
                answer_d.Text = "d";
                good_d.IsChecked = false;
                maxcount++;
                counter++;
            }
            else
            {
                quiz[counter] = questiondata;
                counter++;
                question.Text = quiz[counter][0];
                destinationPath = quiz[counter][1];
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(destinationPath, UriKind.RelativeOrAbsolute);
                image.EndInit();
                ImageBehavior.SetAnimatedSource(ques_image, image);
                timer.Text = quiz[counter][2];
                answer_a.Text = quiz[counter][3];
                if (quiz[counter][7] == "1")
                {
                    good_a.IsChecked = true;
                }
                else
                {
                    good_a.IsChecked = false;
                }
                answer_b.Text = quiz[counter][4];
                if (quiz[counter][8] == "1")
                {
                    good_b.IsChecked = true;
                }
                else
                {
                    good_b.IsChecked = false;
                }
                answer_c.Text = quiz[counter][5];
                if (quiz[counter][9] == "1")
                {
                    good_c.IsChecked = true;
                }
                else
                {
                    good_c.IsChecked = false;
                }
                answer_d.Text = quiz[counter][6];
                if (quiz[counter][10] == "1")
                {
                    good_d.IsChecked = true;
                }
                else
                {
                    good_d.IsChecked = false;
                }
            }

        }

        private void imageupload_Click(object sender, RoutedEventArgs e)
        {
            file.Filter = "Image files|*.bmp;*.jpg;*.jepg;*.gif;*.png";
            file.FilterIndex = 1;
            if (file.ShowDialog() == true)
            {
                Console.WriteLine(file.FileName);
                destinationPath = System.IO.Path.Combine(@"C:\Users\michael\source\repos\Level_8\quiztime\quiztime\images", System.IO.Path.GetFileName(file.FileName));
                if (!File.Exists(destinationPath))
                {
                    File.Copy(file.FileName, System.IO.Path.Combine(@"C:\Users\michael\source\repos\Level_8\quiztime\quiztime\images", System.IO.Path.GetFileName(file.FileName)), true);
                }
                Console.WriteLine(destinationPath);
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(destinationPath, UriKind.RelativeOrAbsolute);
                image.EndInit();
                ImageBehavior.SetAnimatedSource(ques_image, image);
            }
        }

        private void previousquestion_Click(object sender, RoutedEventArgs e)
        {
            counter--;
            if (counter == 0)
            {
                MainWindow win2 = new MainWindow();
                win2.Show();
                this.Close();
            }
            else
            {
                question.Text = quiz[counter][0];
                destinationPath = quiz[counter][1];
                timer.Text = quiz[counter][2];
                answer_a.Text = quiz[counter][3];
                if (quiz[counter][7] == "1")
                {
                    good_a.IsChecked = true;
                }
                else
                {
                    good_a.IsChecked = false;
                }
                answer_b.Text = quiz[counter][4];
                if (quiz[counter][8] == "1")
                {
                    good_b.IsChecked = true;
                }
                else
                {
                    good_b.IsChecked = false;
                }
                answer_c.Text = quiz[counter][5];
                if (quiz[counter][9] == "1")
                {
                    good_c.IsChecked = true;
                }
                else
                {
                    good_c.IsChecked = false;
                }
                answer_d.Text = quiz[counter][6];
                if (quiz[counter][10] == "1")
                {
                    good_d.IsChecked = true;
                }
                else
                {
                    good_d.IsChecked = false;
                }
            }



        }

        private void a_Checked(object sender, RoutedEventArgs e)
        {
            a = 1;
        }

        private void a_unChecked(object sender, RoutedEventArgs e)
        {
            a = 0;
        }

        private void b_Checked(object sender, RoutedEventArgs e)
        {
            b = 1;
        }

        private void b_unChecked(object sender, RoutedEventArgs e)
        {
            b = 0;
        }

        private void c_Checked(object sender, RoutedEventArgs e)
        {
            c = 1;
        }

        private void c_unChecked(object sender, RoutedEventArgs e)
        {
            c = 0;
        }

        private void d_Checked(object sender, RoutedEventArgs e)
        {
            d = 1;
        }

        private void d_unChecked(object sender, RoutedEventArgs e)
        {
            d = 0;
        }

        private void done_Click(object sender, RoutedEventArgs e)
        {
            data md = new data();
            if (editmode)
            {
                md.delete(editid);
            }
            string[] questiondata = new string[11];
            questiondata[0] = question.Text;
            questiondata[1] = destinationPath;
            questiondata[2] = timer.Text;
            questiondata[3] = answer_a.Text;
            questiondata[7] = Convert.ToString(a);
            questiondata[4] = answer_b.Text;
            questiondata[8] = Convert.ToString(b);
            questiondata[5] = answer_c.Text;
            questiondata[9] = Convert.ToString(c);
            questiondata[6] = answer_d.Text;
            questiondata[10] = Convert.ToString(d);
            if (counter + 1 == quiz.Count)
            {
                quiz.Add(questiondata);
            }
            else
            {
                quiz[counter] = questiondata;
            }
            
            md.enterdatabase(quiz);
            MainWindow win2 = new MainWindow();
            win2.Show();
            this.Close();

        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            string quizid = (string)((System.Windows.Controls.Button)sender).Tag;
            data md = new data();
            md.delete(Convert.ToInt32(quizid));
            var myquiz = md.test();
            CQuiz quiz = new CQuiz();
            quiztable.ItemsSource = myquiz;
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            string quizid = (string)((System.Windows.Controls.Button)sender).Tag;
            data md = new data();
            quiz = md.fetcheditor(Convert.ToInt32(quizid));
            editmode = true;
            quizname1.Text = quiz[0][0];
            destinationPath = quiz[0][1];
            quizmaker.Visibility = Visibility.Collapsed;
            quizform.Visibility = Visibility.Visible;
            editid = Convert.ToInt32(quizid);

        }
    }
}
