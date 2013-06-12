using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.DataStructure;

namespace VADG.Parser
{
    public static class LexemeGenerator
    {
        /// <summary>
        /// Used in Analysis, we reverse the compilation by going back to Lexemes
        /// </summary>
        /// <param name="grammarNodeD"></param>
        /// <returns></returns>
        public static List<Lexeme> Reverse(GrammarNodeD grammarNodeD)
        {
            if (grammarNodeD is OperationD)
                return Reverse(grammarNodeD as OperationD);

            List<Lexeme> returnVal = new List<Lexeme>();
            if (grammarNodeD is TerminalD)
            {
                returnVal.Add(Reverse((TerminalD)grammarNodeD));
                return returnVal;
            }

            if (grammarNodeD is NonterminalTailD)
            {
                returnVal.Add(Reverse((NonterminalTailD)grammarNodeD));
                return returnVal;
            }

            if (grammarNodeD is UndefinedD)
            {
                returnVal.Add(Reverse((UndefinedD)grammarNodeD));
                return returnVal;
            }

            throw new NotImplementedException();
        }

        private static List<Lexeme> Reverse(OperationD operationD)
        {
            List<Lexeme> returnVal = new List<Lexeme>();

            for (int i = 0; i < operationD.Children.Count; i++)
            {
                returnVal.AddRange(Reverse(operationD.Children[i]));

                if (i != operationD.Children.Count - 1)
                {
                    // not last
                    if (operationD is ChoiceD)
                        returnVal.Add(new Lexeme(LexemeType.Choice, -1));
                    else if (operationD is ConcatenationD)
                        returnVal.Add(new Lexeme(LexemeType.Concatenation, -1));
                    else
                        throw new NotImplementedException();
                }

            }

            return returnVal;
        }

        private static Lexeme Reverse(TerminalD terminal)
        {
            return new Lexeme(LexemeType.Terminal, terminal.Name, -1);
        }

        private static Lexeme Reverse(NonterminalTailD nont)
        {
            return new Lexeme(LexemeType.Nonterminal, nont.Reference.Name, -1);
        }

        private static Lexeme Reverse(UndefinedD nont)
        {
            return new Lexeme(LexemeType.Undefined, -1);
        }
    }
}
