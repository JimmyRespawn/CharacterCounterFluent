using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LetterCounterWinUI3.Services
{
    public static class ICountLetter
    {
        //Count Letters
        public static async Task<int> AllLetterCountAsync(char[] ch)
        {
            //char[] ch = str.ToCharArray();
            int letterNumber = 0;
            for (int i = 0; i < ch.Length; i++)
            {
                if (Char.IsLetter(ch[i]))
                {
                    letterNumber++;
                }
            }
            return letterNumber;
        }

        public static async Task<int> DigitCountAsync(char[] ch)
        {
            int digitNumber = 0;
            for (int i = 0; i < ch.Length; i++)
            {
                if (Char.IsDigit(ch[i]))
                {
                    digitNumber++;
                }
            }
            return digitNumber;
        }

        public static async Task<int> PunctuationCount(char[] ch)
        {
            int punctuationNumber = 0;
            for (int i = 0; i < ch.Length; i++)
            {
                if (Char.IsPunctuation(ch[i]))
                {
                    punctuationNumber++;
                }
            }
            return punctuationNumber;
        }

        //Count English Words
        public static async Task<int> EnglishWordCountAsync(string str)
        {
            int englishWordNumber;
            string[] words = str.Split(new char[] { ',', ' ', '.', '?', '!', ':', ';', '—', '(', ')', '[', ']', '{', '}', '"', '\'' });
            if (str == "")
                englishWordNumber = 0;
            else
                englishWordNumber = words.Length;
            return englishWordNumber;
        }

        //Count Chinese Letter
        public static async Task<int> ChineseLetterCountAsync(string str)
        {
            int chinesenumber = 0;

            //for (int i = 0; i < ch.Length; i++)
            //{
            //    for (int temp = 0X4e00; temp <= 0X9fa5; temp++)
            //    {
            //        if (ch[i] == Convert.ToChar(temp))
            //        {
            //            chinesenumber++;
            //        }
            //    }
            //}
            Regex regex = new Regex(@"^[\u4E00-\u9fbb]+$");
            for (int i = 0; i < str.Length; i++)
            {
                if (regex.IsMatch(str[i].ToString()))
                {
                    chinesenumber++;
                }
            }

            return chinesenumber;
        }
    }
}
