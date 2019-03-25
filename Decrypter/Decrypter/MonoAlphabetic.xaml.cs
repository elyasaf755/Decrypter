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
    /// Interaction logic for MonoAlphabetic.xaml
    /// </summary>
    public partial class MonoAlphabetic : Page
    {
        public Button SourceButton { get; set; }
        public Button DestinationButton { get; set; }
        public List<char> Alphabet { get; set; }
        public Dictionary<char, char> AlphabetDictionary { get; set; }
        public List<Button> VirtualKeyboardButtons { get; set; }
        public Dictionary<char, double> SingleFrequencies { get; set; }
        public List<TextBlock> SingleFrequenciesTBS { get; set; }
        public List<TextBlock> PairFrequenciesTBS { get; set; }
        public Dictionary<string, double> DoubleFrequencies { get; set; } = new Dictionary<string, double>();

        public MonoAlphabetic()
        {
            InitializeComponent();
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

        private Button FindButton(string letter)
        {
            foreach (Button button in VirtualKeyboardButtons)
            {
                if (button.Content.ToString() == letter.ToString())
                {
                    return button;
                }
            }

            return null;
        }

        private Button FindButtonByChar(char c)
        {
            foreach (Control control in KeyboardGrid.Children)
            {
                if (control is Button)
                {
                    Button button = control as Button;

                    if (button.Content.ToString().ToLower()[0] == c)
                    {
                        return button;
                    }
                }
            }

            return null;
        }

        private bool IsCharsEqual(char c1, char c2)
        {
            if (c1.ToString().ToLower() == c2.ToString().ToLower())
            {
                return true;
            }

            return false;
        }

        private void UpdatePairFrequenciesTable()
        {
            string cipherText = GetRichTextBoxText(CipherRTB).ToLower();
            List<string> pairs = new List<string>();
            pairs = Analysis.FindAllPairs(cipherText);
            DoubleFrequencies = Analysis.FindAllPairsFrequencies(cipherText, pairs);
            DoubleFrequencies = DoubleFrequencies.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            List<string> pairList = new List<string>();
            List<double> freqs = new List<double>();

            double count = 0;

            foreach (var ele in DoubleFrequencies)
            {
                count += ele.Value;
            }

            foreach (var ele in DoubleFrequencies)
            {
                pairList.Add(ele.Key);
                double val = (ele.Value * 100) / count;
                freqs.Add(val);
            }

            for (int i = 0; i < PairFrequenciesTBS.Count; ++i)
            {
                TextBlock tb = PairFrequenciesTBS[i];
                string freqStr;

                if (i >= freqs.Count)
                {
                    tb.Text = "";
                    continue;
                }

                if (Math.Ceiling(freqs[i]) - freqs[i] == 0)
                {
                    freqStr = freqs[i].ToString();
                }
                else
                {
                    freqStr = freqs[i].ToString("0.00");
                }

                tb.Text = pairList[i].ToString() + ": " + freqStr + "%";
            }
        }

        private void SwapChars(char source, char destination)
        {
            for (int i = 0; i < Alphabet.Count; ++i)
            {
                if (Alphabet[i] == source)
                {
                    Alphabet[i] = destination;
                    break;
                }
            }

            AlphabetDictionary[source] = destination;
            #region
            //int sourceIndex = -1;
            //int destinationIndex = -1;
            //for (int i = 0; i < Alphabet.Count; ++i)
            //{
            //    if (Alphabet[i] == source && i != destinationIndex)
            //    {
            //        Alphabet[i] = destination;
            //        sourceIndex = i;
            //    }
            //    else if (Alphabet[i] == destination && i != sourceIndex)
            //    {
            //        Alphabet[i] = source;
            //        destinationIndex = i;
            //    }
            //}
            //if (AlphabetDictionary[source] == destination && AlphabetDictionary[destination] == source)
            //{
            //    AlphabetDictionary[source] = source;
            //    AlphabetDictionary[destination] = destination;
            //}
            //else
            //{
            //    AlphabetDictionary[source] = destination;
            //    AlphabetDictionary[destination] = source;
            //}
            #endregion
        }

        private void UpdateSingleFrequenciesTable()
        {
            string cipherText = GetRichTextBoxText(CipherRTB).ToLower();
            SingleFrequencies = new Dictionary<char, double>();
            for (int i = 0; i < 26; ++i)
            {
                char c = (char)('a' + i);
                SingleFrequencies[c] = Analysis.LetterFrequency(cipherText, c) * 100;
            }
            SingleFrequencies = SingleFrequencies.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            List<char> chars = new List<char>();
            List<double> freqs = new List<double>();

            foreach (var ele in SingleFrequencies)
            {
                chars.Add(ele.Key);
                freqs.Add(ele.Value);
            }

            for (int i = 0; i < SingleFrequenciesTBS.Count; ++i)
            {
                TextBlock tb = SingleFrequenciesTBS[i];
                string freqStr;
                if (Math.Ceiling(freqs[i]) - freqs[i] == 0)
                {
                    freqStr = freqs[i].ToString();
                }
                else
                {
                    freqStr = freqs[i].ToString("0.00");
                }

                tb.Text = chars[i].ToString() + ": " + freqStr + "%";
            }

        }

        private string Decrypt(string str)
        {
            str = str.ToLower();

            string temp = "";
            foreach (char c in str)
            {
                if (c <= 'z' && c >= 'a')
                    temp += AlphabetDictionary[c];
                else
                    temp += c;
            }

            return temp;
        }

        private void CalculateFrequencies()
        {
            string cipherText = GetRichTextBoxText(CipherRTB).ToLower();
            UpdateSingleFrequenciesTable();
            //A.Text = (Analysis.LetterFrequency(cipherText, 'a') * 100).ToString("0.00") + "%";
            //B.Text = (Analysis.LetterFrequency(cipherText, 'b') * 100).ToString("0.00");
            //C.Text = (Analysis.LetterFrequency(cipherText, 'c') * 100).ToString("0.00");
            //D.Text = (Analysis.LetterFrequency(cipherText, 'd') * 100).ToString("0.00");
            //E.Text = (Analysis.LetterFrequency(cipherText, 'e') * 100).ToString("0.00");
            //F.Text = (Analysis.LetterFrequency(cipherText, 'f') * 100).ToString("0.00");
            //G.Text = (Analysis.LetterFrequency(cipherText, 'g') * 100).ToString("0.00");
            //H.Text = (Analysis.LetterFrequency(cipherText, 'h') * 100).ToString("0.00");
            //I.Text = (Analysis.LetterFrequency(cipherText, 'i') * 100).ToString("0.00");
            //J.Text = (Analysis.LetterFrequency(cipherText, 'j') * 100).ToString("0.00");
            //K.Text = (Analysis.LetterFrequency(cipherText, 'k') * 100).ToString("0.00");
            //L.Text = (Analysis.LetterFrequency(cipherText, 'l') * 100).ToString("0.00");
            //M.Text = (Analysis.LetterFrequency(cipherText, 'm') * 100).ToString("0.00");
            //N.Text = (Analysis.LetterFrequency(cipherText, 'n') * 100).ToString("0.00");
            //O.Text = (Analysis.LetterFrequency(cipherText, 'o') * 100).ToString("0.00");
            //P.Text = (Analysis.LetterFrequency(cipherText, 'p') * 100).ToString("0.00");
            //Q.Text = (Analysis.LetterFrequency(cipherText, 'q') * 100).ToString("0.00");
            //R.Text = (Analysis.LetterFrequency(cipherText, 'r') * 100).ToString("0.00");
            //S.Text = (Analysis.LetterFrequency(cipherText, 's') * 100).ToString("0.00");
            //T.Text = (Analysis.LetterFrequency(cipherText, 't') * 100).ToString("0.00");
            //U.Text = (Analysis.LetterFrequency(cipherText, 'u') * 100).ToString("0.00");
            //V.Text = (Analysis.LetterFrequency(cipherText, 'v') * 100).ToString("0.00");
            //W.Text = (Analysis.LetterFrequency(cipherText, 'w') * 100).ToString("0.00");
            //X.Text = (Analysis.LetterFrequency(cipherText, 'x') * 100).ToString("0.00");
            //Y.Text = (Analysis.LetterFrequency(cipherText, 'y') * 100).ToString("0.00");
            //Z.Text = (Analysis.LetterFrequency(cipherText, 'z') * 100).ToString("0.00");
        }

        private double LettersCount(string str)
        {
            str = str.ToLower();

            int i = 0;
            foreach (char c in str)
            {
                if (c <= 'z' && c >= 'a')
                    ++i;
            }

            return i;
        }

        private string GetRichTextBoxText(RichTextBox rtb)
        {
            return new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd).Text;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VirtualKeyboardButtons = new List<Button>();
            foreach (object obj in KeyboardGrid.Children)
            {
                if (obj is StackPanel)
                {
                    StackPanel sp = obj as StackPanel;
                    foreach (object obj1 in sp.Children)
                    {
                        if (obj1 is Button)
                        {
                            Button button = obj1 as Button;

                            button.Background = Brushes.LightGray;
                            VirtualKeyboardButtons.Add(button);
                        }
                    }
                }
            }

            SourceButton = null;
            DestinationButton = null;
            //ABCDEFGHIJKLMNOPQRSTUVWXYZ
            Alphabet = "abcdefghijklmnopqrstuvwxyz".ToList();
            AlphabetDictionary = new Dictionary<char, char>();

            foreach (char c in Alphabet)
            {
                AlphabetDictionary.Add(c, c);
            }

            SingleFrequenciesTBS = new List<TextBlock>();
            foreach (object obj in SingleFrequenciesSP.Children)
            {
                StackPanel sp = (StackPanel)obj;
                foreach (object ele in sp.Children)
                {
                    TextBlock tb = (TextBlock)ele;
                    SingleFrequenciesTBS.Add(tb);
                }
            }

            PairFrequenciesTBS = new List<TextBlock>();
            foreach (object obj in DoubleFrequenciesSP.Children)
            {
                if (obj is TextBlock)
                {
                    TextBlock tb = (TextBlock)obj;
                    PairFrequenciesTBS.Add(tb);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;

            if (!(sender is Button))
                return;

            Button button = sender as Button;

            if (SourceButton == null)
            {
                SourceButton = button;
                SourceButton.Background = Brushes.Green;
            }
            else if (DestinationButton == null)
            {
                DestinationButton = button;
                if (DestinationButton == SourceButton)
                {
                    char ch1 = SourceButton.Content.ToString().ToLower()[0];
                    char ch2 = AlphabetDictionary[ch1];
                    SwapChars(ch1, ch1);
                    SwapChars(ch2, ch2);
                    SourceButton.Background = Brushes.LightGray;
                    DestinationButton = FindButton(ch2.ToString().ToUpper());
                    if (DestinationButton != null)
                    {
                        DestinationButton.Background = Brushes.LightGray;
                    }
                }
                else
                {
                    char source = SourceButton.Content.ToString().ToLower()[0];
                    char destination = DestinationButton.Content.ToString().ToLower()[0];
                    SwapChars(source, destination);
                }

                string decryptedText = Decrypt(GetRichTextBoxText(CipherRTB));
                string plaintText = GetRichTextBoxText(PlainTextRTB);
                if (plaintText != "" && plaintText != null)
                {
                    PlainTextRTB.Document.Blocks.Clear();
                    PlainTextRTB.AppendText(decryptedText);
                }

                NewAlphabet.Text = "";
                char c = 'a';
                foreach (var ele in AlphabetDictionary)
                {
                    NewAlphabet.Text += AlphabetDictionary[c].ToString().ToUpper() + " ";
                    c = (char)(c + 1);
                }

                SourceButton = null;
                DestinationButton = null;
            }

        }

        private void CipherRTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            string cipherText = GetRichTextBoxText(CipherRTB).ToLower();

            if (cipherText == "")
            {
                PlainTextRTB.IsEnabled = false;
                return;
            }

            PlainTextRTB.IsEnabled = true;

            string decryptedText = Decrypt(cipherText);

            PlainTextRTB.Document.Blocks.Clear();
            PlainTextRTB.AppendText(decryptedText);

            UpdateSingleFrequenciesTable();
            UpdatePairFrequenciesTable();
        }

        private void Switch()
        {
            string str = GetRichTextBoxText(CipherRTB).ToLower();
            PlainTextRTB.IsEnabled = true;
            string temp = "";
            foreach (char c in str)
            {
                if (c == 'q')
                    temp += "h";
                else if (c == 'm')
                    temp += "t";
                else if (c == 's')
                    temp += "e";
                else if (c == 'u')
                    temp += "r";
                else if (c == 'k')
                    temp += "a";
                else if (c == 'v')
                    temp += "n";
                else if (c == 'h')
                    temp += "u";
                else if (c == 't')
                    temp += "m";
                else if (c == 'd')
                    temp += "b";
                else if (c == 'b')
                    temp += "o";
                else if (c == 'j')
                    temp += "f";
                else if (c == 'n')
                    temp += "d";
                else if (c == 'x')
                    temp += "i";
                else if (c == 's')
                    temp += "e";
                else if (c == 'p')
                    temp += "c";
                else if (c == 'e')
                    temp += "s";
                else if (c == 'a')
                    temp += "y";
                else if (c == 'y')
                    temp += "p";
                else if (c == 'w')
                    temp += "g";
                else if (c == 'g')
                    temp += "v";
                else if (c == 'i')
                    temp += "x";
                else if (c == 'f')
                    temp += "k";
                else if (c == 'c')
                    temp += "j";
                else if (c == 'r')
                    temp += "q";
                else if (c == 'o')
                    temp += "w";
                else
                    temp += c;
            }

            PlainTextRTB.Document.Blocks.Clear();
            PlainTextRTB.AppendText(temp);
            //PlainTextRTB.Document.Blocks.Clear();
            //PlainTextRTB.AppendText("izrxgrtnwzqf".ToUpper());
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EnglishFrequencies win = new EnglishFrequencies();
            win.Show();
        }
    }
}

