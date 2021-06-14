using System;
using ML.NET.LogisticRegression;

namespace ML.NET.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test");
            BinaryClassifier.RunLR();
            Console.ReadLine();
        }
    }
}
