using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decrypter
{
    public class Analysis
    {
        public static double LetterFrequency(string str, char letter)
        {
            double i = 0;
            foreach (char c in str)
            {
                if (c == letter)
                    ++i;
            }

            double count = LettersCount(str);
            if (count == 0)
                return 0;

            return i / LettersCount(str);
        }

        public static double LettersCount(string str)
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

        public static string LetterFrequencyToString(string str, char letter)
        {
            double letterFrequency = LetterFrequency(str, letter);
            string freqStr;
            if (Math.Ceiling(letterFrequency) - letterFrequency == 0)
            {
                freqStr = letterFrequency.ToString();
            }
            else
            {
                freqStr = letterFrequency.ToString("0.00");
            }
            return letter.ToString().ToUpper() + " " + freqStr + "%";
        }

        public static List<string> FindAllPairs(string str)
        {
            List<string> list = new List<string>();
            if (str.Length < 2)
            {
                return null;
            }

            for (int i = 1; i < str.Length; ++i)
            {
                if ((str[i - 1] <= 'z' && str[i - 1] >= 'a' && str[i] <= 'z' && str[i] >= 'a') ||
                    (str[i - 1] <= 'Z' && str[i - 1] >= 'A' && str[i] <= 'Z' && str[i] >= 'A'))
                {
                    string pair = str[i - 1].ToString() + str[i].ToString();
                    if (IsPairExist(list, pair) == false)
                    {
                        list.Add(pair);
                    }
                }
            }

            return list;
        }

        public static Dictionary<string, double> FindAllPairsFrequencies(string str, List<string> pairs)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();

            foreach (string pair in pairs)
            {
                dic.Add(pair, PairCount(str, pair));
            }

            return dic;
        }
        private static int PairCount(string str, string pair)
        {
            if (str.Length < 2)
            {
                return 0;
            }

            int count = 0;

            for (int i = 1; i < str.Length; ++i)
            {
                if (str[i - 1].ToString() + str[i].ToString() == pair)
                {
                    ++count;
                }
            }

            return count;
        }
        private static bool IsPairExist(List<string> list, string pair)
        {
            foreach (string str in list)
            {
                if (str == pair)
                    return true;
            }

            return false;
        }
    }
}
