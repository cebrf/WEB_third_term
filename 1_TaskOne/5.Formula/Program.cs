using System;

namespace _5.Formula
{
    class Program
    {
        static public double Eps = 0.0000001;
        static void Main(string[] args)
        {
            Console.WriteLine("Choose one of four formula:");
            Console.WriteLine("1) [ | log10(a) + e^a |^(1/2) ]");  //only a
            Console.WriteLine("2) min(sin((a*Pi)/b), cos((b*Pi)/a)) "); // a and b
            Console.WriteLine("3) integral from a to b (8 + 2 * x - x^2)dx");  // a and b
            Console.WriteLine("4) | log2(a) + 1/(b^c) | - tan((Pi*b^c)/(2 * a))"); // a, b, c

            string numberOfFormulaS = Console.ReadLine();
            int numberOfFormula;
            if (!Int32.TryParse(numberOfFormulaS, out numberOfFormula))
            {
                Console.WriteLine("Error!\nSutting down the project");
                return;
            }
            if (numberOfFormula < 1 || numberOfFormula > 4)
            {
                Console.WriteLine("Error!\nSutting down the project");
                return;
            }

            double a = 0, b = 0, c = 0, res = 0;
            string aS, bS, cS;
            Console.Write("a = ");
            aS = Console.ReadLine();
            if (!double.TryParse(aS, out a))
            {
                Console.WriteLine("Error!\nSutting down the project");
                return;
            }

            if (numberOfFormula != 1) // then we need b
            {
                Console.Write("b = ");
                bS = Console.ReadLine();
                if (!double.TryParse(bS, out b))
                {
                    Console.WriteLine("Error!\nSutting down the project");
                    return;
                }
            }
            if (numberOfFormula == 4) // then we need c
            {
                Console.Write("c = ");
                cS = Console.ReadLine();
                if (!double.TryParse(cS, out c))
                {
                    Console.WriteLine("Error!\nSutting down the project");
                    return;
                }
            }
        
            if (numberOfFormula == 1)
            {
                if (a <= Eps)
                {
                    Console.WriteLine("Error!\nSutting down the project");
                    return;
                }
                res = Math.Truncate(Math.Sqrt(Math.Abs(Math.Log10(a) + Math.Exp(a))));
            }
            else if (numberOfFormula == 2)
            {
                if (Math.Abs(a) < Eps || Math.Abs(b) < Eps )
                {
                    Console.WriteLine("Error!\nSutting down the project");
                    return;
                }
                res = Math.Min(Math.Sin((a * Math.PI) / b), Math.Cos((b * Math.PI) / a));
            }
            else if(numberOfFormula == 3)
            {
                if (Math.Abs(b - a) < Eps)
                {
                    Console.WriteLine("Error!\nSutting down the project");
                    return;
                }
                res = 8 * (b - a) + (Math.Pow(b, 2) - Math.Pow(a, 2)) - (Math.Pow(b, 3) - Math.Pow(a, 3));
            }
            else if (numberOfFormula == 4)
            {
                if (a <= Eps || Math.Abs(a) < Eps || Math.Abs(b) < Eps)
                {
                    Console.WriteLine("Error!\nSutting down the project");
                    return;
                }//| log2(a) + 1/(b^c) | - tan((Pi*b^c)/(2 * a))
                res = Math.Abs(Math.Log2(a) + 1 / Math.Pow(b, c)) - Math.Tan((Math.PI * Math.Pow(b, c)) / (2 * a));
            }

            Console.WriteLine("your result = " + res);
        }
    }
}
