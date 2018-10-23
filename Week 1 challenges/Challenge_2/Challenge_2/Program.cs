using System;
using System.Collections.Generic;
using System.Linq;

namespace Challenge_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = new List<double>();
            double averageNumber, sum = 0;
            int numberNumber;

            Console.WriteLine("How many numbers do you want to input?");
            numberNumber = int.Parse(Console.ReadLine());

            for (int i = 0; i < numberNumber; i++)
            {
                Console.WriteLine("Input a number");
                numbers.Add(double.Parse(Console.ReadLine()));
                if (numbers.Contains(0))
                {
                    Console.WriteLine("Stoooooop");
                    averageNumber = sum / numbers.Count;
                    Console.WriteLine("The average of these numbers is " + averageNumber);
                    return;
                }
                sum += numbers[i];
            }

            averageNumber = sum / numbers.Count;
            Console.WriteLine("The average of these numbers is " + averageNumber);
        }
    }
}
