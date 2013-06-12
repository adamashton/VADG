using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.DataStructure;
using VADG.Global;

namespace VADG.GrammarGenerator
{
    class Generator
    {
        private string grammar;

        public string Grammar
        {
            get { return grammar; }
            set { grammar = value; }
        }

        String[] file;
        Random rand;

        /// <summary>
        /// Generates a grammar in text form.
        /// </summary>
        /// <param name="n">Number of rules</param>
        /// <param name="u">The mininum number of symbols per rule</param>
        /// <param name="v">The maximum number of symbols per rule</param>
        /// <param name="weightNont">The percentage of Nonterminals over Terminals in the grammar</param>
        /// <param name="weightConcat">The percentage of concatenation over choice in the grammar</param>
        public Generator(int n, int u, int v, double weightNont, double weightConcat)
        {
            rand = new Random();
            List<String> rules = new List<string>();
            grammar = "";

            for (int i=0; i<n; i++)
                rules.Add(randomWord());
            
            for (int i = 0; i < n; i++)
            {
                               
                grammar += rules[i] + VADG.Global.Settings.AssignmentOperator;
                
                int symbolLength = rand.Next(u, v);
                for (int j = 0; j < symbolLength; j++)
                {
                    // choose nont or t
                    if (rand.NextDouble() <= weightNont)
                        grammar += rules[rand.Next(0, n)];
                    else
                        grammar += VADG.Global.Settings.TerminalEncapsulation + randomWord() + VADG.Global.Settings.TerminalEncapsulation;

                    if (j != symbolLength - 1)
                    {
                        // choose concat or choice
                        if (rand.NextDouble() <= weightConcat)
                            grammar += VADG.Global.Settings.ConcatOperator;
                        else
                            grammar += VADG.Global.Settings.ChoiceOperator;
                    }
                    else
                        grammar += VADG.Global.Settings.EndOfRuleSymbol;
                }

                grammar += "\n\r";
            }
        }

        private String randomWord()
        {
            if (file == null)
                file = System.IO.File.ReadAllLines("brit-a-z.txt");

            

            String returnVal = file[rand.Next(0, file.Length - 1)];
            
            if (returnVal.Contains('\''))
                return randomWord();

            return returnVal;
        }
    }
}
