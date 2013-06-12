using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.Model;

namespace VADG.ParserTest
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    String file = System.IO.File.ReadAllText("grammar.txt");
                    Console.WriteLine(file);

                    GrammarModel model = new GrammarModel();
                    model.setDefinitiveGrammar(file, "S");
                    Console.WriteLine(model.DefinitiveGrammar.PrintGrammar());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Console.ReadLine();
                }
            }
        }
    }
}
