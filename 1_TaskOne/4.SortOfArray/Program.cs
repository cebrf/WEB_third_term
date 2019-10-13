using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4.SortOfArray
{
    class Program
    {
        static void Merge(int[] input, int left, int middle, int right)
        {
            int[] leftArray = new int[middle - left + 1];
            int[] rightArray = new int[right - middle];

            Array.Copy(input, left, leftArray, 0, middle - left + 1);
            Array.Copy(input, middle + 1, rightArray, 0, right - middle);

            int i = 0;
            int j = 0;
            for (int k = left; k < right + 1; k++)
            {
                if (i == leftArray.Length)
                {
                    input[k] = rightArray[j];
                    j++;
                }
                else if (j == rightArray.Length)
                {
                    input[k] = leftArray[i];
                    i++;
                }
                else if (leftArray[i] <= rightArray[j])
                {
                    input[k] = leftArray[i];
                    i++;
                }
                else
                {
                    input[k] = rightArray[j];
                    j++;
                }
            }
        }

        static int[] MergeSort(int[] input, int left, int right)
        {
            if (left < right)
            {
                int middle = (left + right) / 2;

                MergeSort(input, left, middle);
                MergeSort(input, middle + 1, right);

                Merge(input, left, middle, right);
            }
            return input;
        }
        static void Main(string[] args)
        {
            Random rand = new Random();
            int length = rand.Next(2, 20);
            int[] array = new int[length];
            Console.Write("length of array = " + length + "\nYour array: ");
            for (int i = 0; i < length; i++)
            {
                array[i] = rand.Next(-50, 51);
                Console.Write(array[i] + " ");
            }
            Console.WriteLine();
            int[] sortedArray = MergeSort(array, 0, length - 1);
            Console.Write("decreasing or increasing order? Choose 'd' or 'i':  ");
            string order = Console.ReadLine();
            if (order == "d")
            {
                Console.Write("Sorted array: ");
                for (int i = length - 1; i >= 0; i--)
                {
                    Console.Write(sortedArray[i] + " ");
                }
            }
            else if (order == "i")
            {
                Console.Write("Sorted array: ");
                for (int i = 0; i < length; i++)
                {
                    Console.Write(sortedArray[i] + " ");
                }
            }
            else
            {
                Console.WriteLine("Error!\nSutting down the priject");
                return;
            }
        }
    }
}
