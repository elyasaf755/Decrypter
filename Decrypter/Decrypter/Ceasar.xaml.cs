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
    /// Interaction logic for Ceasar.xaml
    /// </summary>
    public partial class Ceasar : Page
    {
        public Ceasar()
        {
            InitializeComponent();
        }

        private string GetRichTextBoxText(RichTextBox rtb)
        {
            return new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd).Text;
        }

        private void SendKey(Key key)
        {
            if (Keyboard.PrimaryDevice != null)
            {
                if (Keyboard.PrimaryDevice.ActiveSource != null)
                {
                    var ea = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key)
                    {
                        RoutedEvent = Keyboard.KeyDownEvent
                    };
                    InputManager.Current.ProcessInput(ea);
                }
            }
        }

        private void CipherRTB_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendKey(Key.Back);
                CipherRTB.AppendText("\r");
                SendKey(Key.Down);
            }
        }

        private void Decrypt()
        {
            string cipherText = GetRichTextBoxText(CipherRTB).ToLower();

            if (cipherText == "")
            {
                PlainTextRTB.IsEnabled = false;
                return;
            }

            PlainTextRTB.IsEnabled = true;

            string decryptedText = Decrypt(Convert.ToInt32(KeySlider.Value), cipherText);

            PlainTextRTB.Document.Blocks.Clear();
            PlainTextRTB.AppendText(decryptedText);
        }

        private void CipherRTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            Decrypt();
        }

        private string Decrypt(int key, string str)
        {
            str = str.ToLower();
            string temp = "";

            foreach (char c in str)
            {
                if (c > 'z' || c < 'a')
                    temp += c;
                else if (c + key > 'z')
                {
                    temp += (char)(c + key - 'z' + 'a' - 1);
                }
                else
                {
                    temp += (char)(c + key);
                }
            }

            return temp;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            KeyValueTB.Text = Convert.ToInt32(KeySlider.Value).ToString();

            Decrypt();
        }
    }
}
