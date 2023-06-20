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

namespace quiztime
{
    /// <summary>
    /// Interaction logic for display.xaml
    /// </summary>
    public partial class display : Window
    {
        public display()
        {
            InitializeComponent();

        }


          
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           this.WindowState = WindowState.Maximized;
/*            quizplayer.Width = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            quizplayer.Height = (int)System.Windows.SystemParameters.PrimaryScreenHeight;*/

            
        }
    }
}
