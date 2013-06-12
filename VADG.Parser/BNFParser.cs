using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.DataStructure;

namespace VADG.Parser
{
    /// <summary>
    /// The parser will parse the given string into sets of nonterminals.
    /// You will have to choose the Start symbol and make the definitive grammar.
    /// </summary>
    public class BNFParser
    {
        private List<NonterminalHeadD> rules;
        /// <summary>
        /// The list of rules found in this grammar.
        /// </summary>
        public List<NonterminalHeadD> Rules
        {
            get { return rules; }
        }

        private String errorText;
        /// <summary>
        ///  Access this after an unsuccessful parse.
        /// </summary>
        public String ErrorText
        {
            get { return errorText; }
        }

        private String fileText;

        /// <summary>
        /// Returns true if the parser successfully parses a BNF file. Else, check errorText.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool Parse(String input)
        {
            try
            {
                fileText = input.Trim(); ;
                rules = new List<NonterminalHeadD>();

                if (String.IsNullOrEmpty(fileText))
                    throw new Exception("Input is null or empty.");

                // lexical analysis
                LexicalAnalyser lexA = new LexicalAnalyser(fileText);
                if (!lexA.Analyse())
                {
                    errorText = lexA.ErrorMsg;
                    return false;
                }

                // syntactic analysis
                SyntaxAnalyser synA = new SyntaxAnalyser(lexA);
                if (!synA.Analyse())
                {
                    errorText = synA.ErrorMsg;
                    return false;
                }

                // semantic analysis
                SemanticAnalyser semA = new SemanticAnalyser(synA);

                if (!semA.Analyse())
                {
                    errorText = semA.ErrorMsg;
                    return false;
                }

                // save the rules form semantic analyser :-)                
                rules = semA.Rules;
                if (rules == null || rules.Count == 0)
                    throw new Exception("The semantic analyser did not find any rules in the input text.");

                return true;
            }
            catch (Exception e)
            {
                errorText = e.Message;
                return false;
            }
        }
    }
}
