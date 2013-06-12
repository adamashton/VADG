using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.Parser;
using VADG.DataStructure;

namespace VADG.Model.Analysis
{
    public class SubRule
    {
        public SubRule(List<Lexeme> rulesIn, NonterminalHeadD nontHeadIn)
        {
            Rule = rulesIn;
            NonterminalHeadD = nontHeadIn;
        }

        public List<Lexeme> Rule;
        public NonterminalHeadD NonterminalHeadD;

        public int SymbolCount
        {
            get
            {
                int count = 0;
                foreach (Lexeme lexeme in Rule)
                {
                    if (lexeme.Type == LexemeType.Nonterminal || lexeme.Type == LexemeType.Terminal || lexeme.Type == LexemeType.Undefined)
                        count++;
                }

                return count;
            }
        }

        public List<Lexeme> getSubRule(int position, int stepSize)
        {
            // get to correct position
            int correctPosition = position*2;

            int correctStepSize = stepSize*2 - 1; // since every symbol is seperated by an operator, stepsize doubles - 1
            List<Lexeme> returnVal = new List<Lexeme>();

            for (int i = correctPosition; i < correctPosition + correctStepSize; i++)
                returnVal.Add(Rule[i]);

            return returnVal;
        }

        public override string ToString()
        {
            String returnVal = "";
            
            foreach (Lexeme lexeme in Rule)
            {
                returnVal += lexeme.ToValueString();
            }

            return returnVal;
        }
        
    }
}
