using MySqlX.XDevAPI.Relational;
using quiztime.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Data;
using WpfAnimatedGif;
using Org.BouncyCastle.Utilities.Encoders;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace quiztime
{
    /// <summary>
    /// Interaction logic for controls.xaml
    /// </summary>
    public partial class controls : Window
    {
        List<string[]> question;
        int counter;
        int pauser;
        Boolean checkanswer;
        Boolean checkround;
        DispatcherTimer _timer;
        TimeSpan _time;
        Boolean timeron;
        public controls()
        {
            InitializeComponent();
            data md = new data();
            var myquiz = md.test();
            CQuiz quiz = new CQuiz();
            quiztable.ItemsSource = myquiz;
        }


        private void close_Click(object sender, RoutedEventArgs e)
        {

            System.Environment.Exit(0);
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            timeron = false;

            if (checkanswer)
            {
                checkround = true;
            }
            counter = 0;
            pauser = 1;
            string quizid = (string)((System.Windows.Controls.Button)sender).Tag;
            data md = new data();
            question = md.getquestions(Convert.ToInt32(quizid));
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window.GetType() == typeof(display))
                {
                    _time = TimeSpan.FromSeconds(Convert.ToDouble(question[0][11]));

                    _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                    {
                        (window as display).timer.Content = _time.ToString("c");
                        if (_time == TimeSpan.Zero) _timer.Stop();
                        if (_time == TimeSpan.Zero) next_Click(null, null);
                        _time = _time.Add(TimeSpan.FromSeconds(-1));
                    }, System.Windows.Application.Current.Dispatcher);
                    _timer.Stop();
                    (window as display).selectimage.Visibility = Visibility.Collapsed;
                    (window as display).quizplayer.Visibility = Visibility.Visible;
                    (window as display).question.Content = question[0][0];
                    (window as display).answer_a.Content = question[0][1];
                    (window as display).answer_b.Content = question[0][2];
                    (window as display).answer_c.Content = question[0][3];
                    (window as display).answer_d.Content = question[0][4];
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(question[counter][5], UriKind.RelativeOrAbsolute);
                    image.EndInit();
                    ImageBehavior.SetAnimatedSource((window as display).ques_image, image);


                    var image2 = new BitmapImage();
                    image2.BeginInit();
                    image2.UriSource = new Uri(question[0][6], UriKind.RelativeOrAbsolute);
                    image2.EndInit();
                    ImageBehavior.SetAnimatedSource((window as display).pauseimage2, image2);
                }
            }
            quizselector.Visibility = Visibility.Collapsed;
            quizplaying.Visibility = Visibility.Visible;
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {

            if (checkanswer && checkround)
            {
                checkround = false;
                if (question[counter][7] == "0")
                {
                    foreach (Window window in System.Windows.Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(display))
                        {
                            (window as display).answer_a.Background = Brushes.Gray;
                        }
                    }
                }
                if (question[counter][8] == "0")
                {
                    foreach (Window window in System.Windows.Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(display))
                        {
                            (window as display).answer_b.Background = Brushes.Gray;
                        }
                    }
                }

                if (question[counter][9] == "0")
                {
                    foreach (Window window in System.Windows.Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(display))
                        {
                            (window as display).answer_c.Background = Brushes.Gray;
                        }
                    }
                }
                if (question[counter][10] == "0")
                {
                    foreach (Window window in System.Windows.Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(display))
                        {
                            (window as display).answer_d.Background = Brushes.Gray;
                        }
                    }
                }

            }
            else
            {
                checkround = true;
                foreach (Window window in System.Windows.Application.Current.Windows)
                {
                    if (window.GetType() == typeof(display))
                    {
                        (window as display).answer_a.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x74, 0xE3, 0xC6)); 
                        (window as display).answer_b.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x74, 0xE3, 0xC6));
                        (window as display).answer_c.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x74, 0xE3, 0xC6));
                        (window as display).answer_d.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x74, 0xE3, 0xC6));
                    }
                }
                counter++;
                if (question.Count == counter)
                {
                    quizselector.Visibility = Visibility.Visible;
                    quizplaying.Visibility = Visibility.Collapsed;

                    foreach (Window window in System.Windows.Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(display))
                        {
                            (window as display).selectimage.Visibility = Visibility.Visible;
                            (window as display).quizplayer.Visibility = Visibility.Collapsed;
                            (window as display).pauseimage.Visibility = Visibility.Collapsed;
                            pauser = 1;
                        }
                    }
                }
                else
                {
                    foreach (Window window in System.Windows.Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(display))
                        {
                            _time = TimeSpan.FromSeconds(Convert.ToDouble(question[counter][11]));

                            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                            {
                                (window as display).timer.Content = _time.ToString("c");
                                if (_time == TimeSpan.Zero) _timer.Stop();
                                if (_time == TimeSpan.Zero) next_Click(null, null);
                                _time = _time.Add(TimeSpan.FromSeconds(-1));
                            }, System.Windows.Application.Current.Dispatcher);
                            if (!timeron)
                            {
                                _timer.Stop();
                            }
                            (window as display).question.Content = question[counter][0];
                            (window as display).answer_a.Content = question[counter][1];
                            (window as display).answer_b.Content = question[counter][2];
                            (window as display).answer_c.Content = question[counter][3];
                            (window as display).answer_d.Content = question[counter][4];
                            (window as display).pauseimage.Visibility = Visibility.Collapsed;
                            (window as display).quizplayer.Visibility = Visibility.Visible;
                            pauser = 1;
                            var image = new BitmapImage();
                            image.BeginInit();
                            image.UriSource = new Uri(question[counter][5], UriKind.RelativeOrAbsolute);
                            image.EndInit();
                            ImageBehavior.SetAnimatedSource((window as display).ques_image, image);

                        }
                    }
                }
            }
        }

        private void previous_Click(object sender, RoutedEventArgs e)
        {
            if (!timeron)
            {
                _timer.Stop();
            }
            {
                if (counter == 0)
                {
                    quizselector.Visibility = Visibility.Visible;
                    quizplaying.Visibility = Visibility.Collapsed;

                    foreach (Window window in System.Windows.Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(display))
                        {
                            (window as display).selectimage.Visibility = Visibility.Visible;
                            (window as display).quizplayer.Visibility = Visibility.Collapsed;
                            (window as display).pauseimage.Visibility = Visibility.Collapsed;
                            pauser = 1;
                        }
                    }
                }
                else
                {
                    counter--;
                    foreach (Window window in System.Windows.Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(display))
                        {
                            _time = TimeSpan.FromSeconds(Convert.ToDouble(question[counter][11]));

                            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                            {
                                (window as display).timer.Content = _time.ToString("c");
                                if (_time == TimeSpan.Zero) _timer.Stop();
                                if (_time == TimeSpan.Zero) next_Click(null, null);
                                _time = _time.Add(TimeSpan.FromSeconds(-1));
                            }, System.Windows.Application.Current.Dispatcher);
                            if (!timeron)
                            {
                                _timer.Stop();
                            }

                            (window as display).question.Content = question[counter][0];
                            (window as display).answer_a.Content = question[counter][1];
                            (window as display).answer_b.Content = question[counter][2];
                            (window as display).answer_c.Content = question[counter][3];
                            (window as display).answer_d.Content = question[counter][4];
                            (window as display).pauseimage.Visibility = Visibility.Collapsed;
                            (window as display).quizplayer.Visibility = Visibility.Visible;
                            pauser = 1;
                            var image = new BitmapImage();
                            image.BeginInit();
                            image.UriSource = new Uri(question[counter][5], UriKind.RelativeOrAbsolute);
                            image.EndInit();
                            ImageBehavior.SetAnimatedSource((window as display).ques_image, image);

                        }
                    }
                }
            }
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            if (pauser == 0)
            {
                foreach (Window window in System.Windows.Application.Current.Windows)
                {
                    if (window.GetType() == typeof(display))
                    {
                        (window as display).pauseimage.Visibility = Visibility.Collapsed;
                        (window as display).quizplayer.Visibility = Visibility.Visible;
                        pauser = 1;
                        if (timeron)
                        {
                            _timer.Start();
                        }
                    }
                }
            }

            else if (pauser == 1)
            {
                foreach (Window window in System.Windows.Application.Current.Windows)
                {
                    if (window.GetType() == typeof(display))
                    {
                        if (timeron)
                        {
                            _timer.Stop();
                        }
                        (window as display).pauseimage.Visibility = Visibility.Visible;
                        (window as display).quizplayer.Visibility = Visibility.Collapsed;
                        pauser = 0;
                    }
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            checkanswer = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            checkanswer = false;
        }

        private void timer_Click(object sender, RoutedEventArgs e)
        {
            if (timeron)
            {
                _timer.Stop();
                timeron = false;
                foreach (Window window in System.Windows.Application.Current.Windows)
                {
                    if (window.GetType() == typeof(display))
                    {
                        (window as display).timer.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else
            {
                _timer.Start();
                timeron = true;
                foreach (Window window in System.Windows.Application.Current.Windows)
                {
                    if (window.GetType() == typeof(display))
                    {
                        (window as display).timer.Visibility = Visibility.Visible;
                    }
                }
            }
        }
    }
}
