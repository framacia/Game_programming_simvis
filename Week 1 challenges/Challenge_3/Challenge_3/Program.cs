using System;

namespace Challenge_3
{
    class Program
    {
        static void Main(string[] args)
        {
            bool cnt = true;
            int number = 0;

            string text = System.IO.File.ReadAllText(@"C:\Users\F.MaciaVarela1\source\repos\Challenge_3\text_1.txt");

            string s1 = "the quick brown fox jumps over the lazy dog";
            string s2 = "the";
            string userSearch;

            Console.WriteLine("What would you like to search?");
            userSearch = Console.ReadLine();

            while (cnt)
            {
                Console.WriteLine(text.IndexOf(userSearch, number));
                number = 1 + text.IndexOf(userSearch, number);
                if (text.IndexOf(userSearch, number) == -1)
                {
                    cnt = false;
                }
                else
                {
                    cnt = true;
                }
            }
        }
    }
}