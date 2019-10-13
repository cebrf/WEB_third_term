using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2.ArithmeticOperations
{
    class Program
    {
        static void Main(string[] args)
        {
            bool next = true;
            while (next)
            {
                Console.Write("Choose operation: ");
                string operation = Console.ReadLine();
                if (operation != "+" && operation != "-" && operation != "*" && operation != "/")
                {
                    Console.WriteLine("Error!\nSutting down the project");
                    next = false;
                    break;
                }
                Console.Write("Choose first operator: ");
                string operatorOneS = Console.ReadLine();
                int operatorOne;
                if (!Int32.TryParse(operatorOneS, out operatorOne))
                {
                    Console.WriteLine("Error!\nSutting down the project");
                    next = false;
                    break;
                }
                Console.Write("Choose second operator: ");
                string operatorTwoS = Console.ReadLine();
                int operatorTwo;
                if (!Int32.TryParse(operatorTwoS, out operatorTwo))
                {
                    Console.WriteLine("Error!\nSutting down the project");
                    next = false;
                    break;
                }
                switch (operation)
                {
                    case "+":
                        Console.WriteLine("Result =  " + (operatorOne + operatorTwo));
                        break;
                    case "-":
                        Console.WriteLine("Result =  " + (operatorOne - operatorTwo));
                        break;
                    case "*":
                        Console.WriteLine("Result =  " + (operatorOne * operatorTwo));
                        break;
                    case "/":
                        if (operatorTwo == 0)
                        {
                            Console.WriteLine("Error! Dividing by zero!\nSutting down the project");
                            next = false;
                            break;
                        }
                        Console.WriteLine("Result =  " + (operatorOne / operatorTwo));
                        break;
                }
                Console.Write("Continue? Yes or No:  ");
                string stop = Console.ReadLine();
                if (stop.Length == 0)
                {
                    Console.WriteLine("Error!\nSutting down the priject");
                    next = false;
                    break;
                }
                else
                {
                    if (stop == "Yes" || stop == "yes" || stop == "y" || stop == "Y")
                    {
                        next = true;
                    }
                    else if (stop == "No" || stop == "no" || stop == "n" || stop == "N")
                    {
                        next = false;
                    }
                    else
                    {
                        Console.WriteLine("Error!\nSutting down the priject");
                        next = false;
                        break;
                    }
                }
            }
        }
    }
}
