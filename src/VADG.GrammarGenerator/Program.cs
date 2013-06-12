using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.GrammarGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Generator g = new Generator(50, 10, 50, 0.5, 0.8);

            Console.WriteLine(g.Grammar);
            

            System.IO.File.WriteAllText(@"C:\OpenShare\VADG\generatedGrammar.txt", g.Grammar);

            Console.ReadLine();
        }
    }
}
