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

namespace Decrypter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MonoAlphabetic MonoAlphabeticPage { get; set; } = new MonoAlphabetic();
        public Ceasar CeasarPage { get; set; } = new Ceasar();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MonoAlphabetic_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = MonoAlphabeticPage;
        }

        private void Ceasar_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = CeasarPage;
        }
    }
}
