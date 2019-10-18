using System;

namespace _6.TextProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] delimiterChars = { ',', '.', ':', ';', '-', '\"', '\'' };
            Console.Write("Enter your text:    ");
            string text = Console.ReadLine();
            string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            Int64 numberOfWords = 0;
            Int64 numberOfCharactersWithoutSpaces = 0;
            double ratioOfCharactersToWords = 0;
            string wordFromLastCharactersOfWords = "";
            
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 1 || Char.IsLetterOrDigit(words[i][0]))
                {
                    numberOfCharactersWithoutSpaces += words[i].Length;
                    numberOfWords++;
                    string[] subword = words[i].Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < subword.Length; j++)
                    {
                        if (subword[j].Length > 1 || Char.IsLetterOrDigit(subword[j][0]))
                        {
                            wordFromLastCharactersOfWords += subword[j][subword[j].Length - 1];
                        }
                    }
                }
            }
            
            ratioOfCharactersToWords = Convert.ToDouble(numberOfCharactersWithoutSpaces) / Convert.ToDouble(numberOfWords);
            Console.WriteLine(numberOfWords);
            Console.WriteLine(numberOfCharactersWithoutSpaces);
            Console.WriteLine(String.Format("{0:0.00}", ratioOfCharactersToWords));  //Math.Round(ratioOfCharactersToWords, 2));
            Console.WriteLine(wordFromLastCharactersOfWords);
        }
    }
}
