using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3.GenerationOfArray
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            Console.Write("length of array: ");
            string lengthS = Console.ReadLine();
            int length;
            if (!Int32.TryParse(lengthS, out length))
            {
                Console.WriteLine("Error!\nSutting down the priject");
                return;
            }
            Console.Write("min val: ");
            string minValS = Console.ReadLine();
            int minVal;
            if (!Int32.TryParse(minValS, out minVal))
            {
                Console.WriteLine("Error!\nSutting down the priject");
                return;
            }
            Console.Write("max val: ");
            string maxValS = Console.ReadLine();
            int maxVal;
            if (!Int32.TryParse(maxValS, out maxVal))
            {
                Console.WriteLine("Error!\nSutting down the priject");
                return;
            }

            int[] array = new int[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = rand.Next(minVal, maxVal + 1);
            }

            Console.Write("array: ");
            for (int i = 0; i < length; i++)
            {
                Console.Write(array[i] + " ");
            }
        }
    }
}
