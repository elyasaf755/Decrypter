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

namespace Decrypter
{
    /// <summary>
    /// Interaction logic for EnglishFrequencies.xaml
    /// </summary>
    public partial class EnglishFrequencies : Window
    {
        public EnglishFrequencies()
        {
            InitializeComponent();
            ImageBrush background1 = new ImageBrush();
            background1.ImageSource = new BitmapImage(new Uri(@"SingleFrequencies.jpg", UriKind.Relative));
            First.Background = background1;
            ImageBrush background2 = new ImageBrush();
            background2.ImageSource = new BitmapImage(new Uri(@"PairFrequencies.jpg", UriKind.Relative));
            Second.Background = background2;
        }
    }
}
