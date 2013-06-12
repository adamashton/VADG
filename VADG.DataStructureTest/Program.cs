using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.Model;
using VADG.DataStructure;
using VADG.Global;

namespace VADG.DataStructureTest
{
    class Program
    {
        public static void Main(String[] args)
        {
            while (true)
            {
                try
                {
                    GrammarModel model = new GrammarModel(Console.ReadLine());

                    VADG.Model.Analysis.FactorisationAnalyser.Analyse(model.DefinitiveGrammar);

                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
