using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.DataStructure;
using System.Collections;

namespace VADG.Parser
{
    /// <summary>
    /// Will obtain a list of the nonterminalhead's that are in the grammar.
    /// Note the rules have not been made yet, that is job of semantic analyser.
    /// </summary>
    internal class SyntaxAnalyser
    {
        private ArrayList productionRulesTail;
        /// <summary>
        /// An Array of List<Lexeme> production rules
        /// </summary>
        public ArrayList ProductionRulesTail
        {
            get { return productionRulesTail; }
        }

        /// <summary>
        /// A list of Lexemes that are heads of the ProductionRulesTails
        /// </summary>
        private List<Lexeme> productionRulesHead;
        public List<Lexeme> ProductionRulesHead
        {
            get { return productionRulesHead; }
        }



        private List<Lexeme> rulesLexemes;
        private bool inRule;

        private LexicalAnalyser lexA;
        private List<Lexeme> lexemes;

        private int current = 0;

        private Lexeme currentLexeme
        {
            get { return lexemes[current]; }
        }

        private String errorMsg = "No Errors.";
        public String ErrorMsg
        {
            get { return errorMsg; }
        }

        public SyntaxAnalyser(LexicalAnalyser lexAIn)
        {
            Init(lexAIn);
        }

        private void Init(LexicalAnalyser lexAIn)
        {
            lexA = lexAIn;

            current = 0;

            productionRulesHead = new List<Lexeme>();
            productionRulesTail = new ArrayList();            

            inRule = false;
        }

        /// <summary>
        /// Swallow the lexeme tokens, saving all the nonterminals and stream of rules from the stream.
        /// </summary>
        /// <returns></returns>
        public bool Analyse()
        {
            try
            {
                // get lexemes from lexical analyser
                if (lexemes == null)
                    lexemes = lexA.Lexemes;

                // Grammar -> Rules, EOF
                Rules();

                if (currentLexeme.Type != LexemeType.EOF)
                {
                    errorMsg = "Expected End of File, found character of type " + currentLexeme + " on line " + currentLexeme.LineNumber;
                    return false;
                }

                errorMsg = "No Errors.";
                return true;
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return false;
            }
        }

        private void GetNext()
        {
            current++;
        }

        private bool accept(LexemeType typeIn)
        {
            if (currentLexeme.Type == typeIn)
            {
                if (inRule)
                    rulesLexemes.Add(currentLexeme);

                GetNext();
                return true;
            }

            //int errLineNo = currentLexeme.LineNumber;
            string errMsg = "Expected " + typeIn + " found " + currentLexeme + " on line " + currentLexeme.LineNumber
                            + "\nLine: " + lexA.GetLine(currentLexeme.LineNumber);                                                                               

            throw new Exception(errMsg);
        }

        // Rules -> Rule | Rule, Rules
        public bool Rules()
        {
            try
            {
                Rule();

                // start of rule
                if (currentLexeme.Type == LexemeType.Nonterminal)
                {
                    // Rules -> Rule, Rules
                    Rules();
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        // Rule -> Nonterminal, AssignmentOp, Symbols, EndOfRuleSymbol
        private bool Rule()
        {
            Lexeme ruleHead;

            try
            {
                ruleHead = NonterminalHead();
                AssignmentOp();

                // used to add all the symbols you come across
                inRule = true;
                rulesLexemes = new List<Lexeme>();

                Symbols();
            }
            catch (Exception e)
            {
                throw new Exception("Could not parse a rule.\n" + e.Message);
            }
            finally
            {
                inRule = false;
            }

            try
            {
                // extra help from compiler errorMsg.
                EndOfRule();

                // successfully parsed a rule
                productionRulesHead.Add(ruleHead);
                productionRulesTail.Add(rulesLexemes);
                rulesLexemes = null;

                return true;
            }
            catch (Exception e)
            {

                throw new Exception("Could not parse a rule, have you missed a semi-colon in previous rule?\n" + e.Message);
            }
            finally
            {
                inRule = false;
            }
        }

        

        // Symbols -> Symbol | Symbol, operator, Symbols
        private bool Symbols()
        {
            try
            {
                Symbol();

                //if (currentLexeme.Type == LexemeType.Concatenation || currentLexeme.Type == LexemeType.Choice)
                if (currentLexeme.Type != LexemeType.EndOfRule)
                {
                    // Symbols -> Symbol, operator, Symbols
                    SymbolInfixOperator();
                    Symbols();
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // SymbolInfixOperator -> ConcatanationOp | ChoiceOp
        private bool SymbolInfixOperator()
        {
            try
            {
                if (currentLexeme.Type == LexemeType.Choice)
                    return accept(LexemeType.Choice);

                if (currentLexeme.Type == LexemeType.Concatenation)
                    return accept(LexemeType.Concatenation);

                if (VADG.Global.Settings.ConcatIsWhiteSpace())
                    // no concat operator!
                    return true;

                int errLineNo = currentLexeme.LineNumber;
                string errMsg = "Unexpected token " + currentLexeme.Type + " on line " + errLineNo
                                + "\nLine " + errLineNo + ": " + lexA.GetLine(errLineNo);

                throw new Exception(errMsg);
            }
            catch (Exception e)
            {
                throw new Exception("Expected concatenation or choice operator.\n" + e.Message);
            }
        }

        // Symbol -> Nonterminal | Terminal
        private bool Symbol()
        {
            try
            {
                if (currentLexeme.Type == LexemeType.Nonterminal)
                    return Nonterminal();

                if (currentLexeme.Type == LexemeType.Terminal)
                    return Terminal();

                if (currentLexeme.Type == LexemeType.Undefined)
                    return Undefined();

                int errLineNo = currentLexeme.LineNumber;
                string errMsg = "Unexpected token " + currentLexeme.Type + " on line " + errLineNo
                                + "\nLine " + errLineNo + ": " + lexA.GetLine(errLineNo);

                throw new Exception(errMsg);
            }
            catch (Exception e)
            {
                throw new Exception("Expected Nonterminal or Terminal symbol.\n" + e.Message);
            }
        }

        private bool Nonterminal()
        {
            return accept(LexemeType.Nonterminal);
        }

        private Lexeme NonterminalHead()
        {
            if (currentLexeme.Type == LexemeType.Nonterminal)
            {
                // we have a nonterminal head :)
                Lexeme returnVal = currentLexeme;

                GetNext();

                return returnVal;
            }
            else
                // handle error consistently with the accept method, will throw exception
                accept(LexemeType.Nonterminal);
            
            // assert: never reached
            throw new Exception("Should not reach here, should fail on accept method above.");
        }

        private bool Terminal()
        {
            return accept(LexemeType.Terminal);
        }

        private bool AssignmentOp()
        {
            return accept(LexemeType.Assignment);
        }

        private bool EndOfRule()
        {
            return accept(LexemeType.EndOfRule);
        }

        private bool EndOfFile()
        {
            return accept(LexemeType.EOF);
        }

        private bool Undefined()
        {
            return accept(LexemeType.Undefined);
        }

    }
}
