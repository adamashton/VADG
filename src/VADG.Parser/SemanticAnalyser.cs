using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.DataStructure;
using System.Collections;

namespace VADG.Parser
{
    /// <summary>
    /// Creates the rules from a successful syntatic analysis.
    /// </summary>
    internal class SemanticAnalyser
    {
        private List<NonterminalHeadD> rules;
        public List<NonterminalHeadD> Rules
        {
            get { return rules; }
        }
        
        private SyntaxAnalyser synA;

        private String errorMsg = "No Errors.";
        public String ErrorMsg
        {
            get { return errorMsg; }
        }

        public SemanticAnalyser(SyntaxAnalyser synAIn)
        {
            Init(synAIn);            
        }

        private void Init(SyntaxAnalyser synAIn)
        {
            synA = synAIn;
            rules = new List<NonterminalHeadD>();
        }

        public bool Analyse()
        {
            try
            {
                if (synA.ProductionRulesHead == null || synA.ProductionRulesHead.Count == 0
                    || synA.ProductionRulesTail == null || synA.ProductionRulesTail.Count == 0)
                {
                    errorMsg = "There are no rules found. Possible empty file / Syntactic Analyser failed?";
                    return false;
                }

                // create nonterminal heads
                foreach (Lexeme nonterminalLexeme in synA.ProductionRulesHead)
                {
                    //if (getNonterminalHead(nonterminalLexeme.Value) == null)
                    //{
                        // create the nont, add to rules
                    // create the nonterminal
                
                    
                    NonterminalHeadD nontHead = new NonterminalHeadD(nonterminalLexeme.Value);
                    nontHead.Rule = new ChoiceLineD(nontHead);
                    rules.Add(nontHead);                    
                    // otherwise do nothing now, add to choiceLineD when analysing rules
                }

                // create tree structure for each rule
                for (int i=0; i<synA.ProductionRulesHead.Count; i++)
                {
                    Lexeme nonterminalLexeme = synA.ProductionRulesHead[i];
                    NonterminalHeadD nontTerminalHead = getNonterminalHead(nonterminalLexeme.Value);
                    if (nontTerminalHead == null)
                        throw new Exception("There was an unknown error during the processing of rules");

                    List<Lexeme> productionRuleLexemes = synA.ProductionRulesTail[i] as List<Lexeme>;
                    if (productionRuleLexemes == null)
                        throw new Exception("The given rule " + nontTerminalHead.Name + " has no production rule associated with it.");
                    

                    CreateRuleDataStructure(productionRuleLexemes, nontTerminalHead.Rule);
                    //nontTerminalHead.Rule.appendChild(ruleStructure);
                }

                return true;
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return false;
            }
        }

        private void createNonterminalD(Lexeme nonterminalLexeme)
        {
            NonterminalHeadD nont = new NonterminalHeadD(nonterminalLexeme.Value);
            nont.Rule = new ChoiceLineD(nont);
            rules.Add(nont);
        }

        /// <summary>
        /// Returns the rule structure, without the ChoiceLineD
        /// </summary>
        /// <param name="productionRuleLexemes"></param>
        /// <returns></returns>
        private void CreateRuleDataStructure(List<Lexeme> productionRuleLexemes, ChoiceLineD choiceLineD)
        {
            //GrammarNodeD returnVal;            
            OperationD current = choiceLineD; // points to which level we are at
            //returnVal = current;
            
            // Split by Choice as this has the highest precedence
            List<Lexeme> choiceLexemes = new List<Lexeme>();
            ArrayList choices = new ArrayList();
            
            foreach (Lexeme lexeme in productionRuleLexemes)
            {
                if (lexeme.Type == LexemeType.Choice)
                {
                    choices.Add(choiceLexemes);
                    choiceLexemes = new List<Lexeme>();
                }
                else
                {
                    choiceLexemes.Add(lexeme);
                }
            }

            // add last choice list
            choices.Add(choiceLexemes);

            // put in a choiceD
            ChoiceD choiceD = new ChoiceD(current);
            current.appendChild(choiceD);
            current = choiceD; // move down a level
            
            // add the concatenated items for each Choice
            foreach (List<Lexeme> concatLexemes in choices)
            {
                ConcatenationD concatenation = new ConcatenationD(current);               

                foreach (Lexeme lexeme in concatLexemes)
                {
                    if (lexeme.Type == LexemeType.Nonterminal)
                    {
                        // create nonterminal tail from the nonterminalhead
                        NonterminalHeadD nontHead = getNonterminalHead(lexeme.Value);
                        if (nontHead == null)
                            throw new Exception("Could not find a defined rule for the Nonterminal: " + lexeme.Value);

                        NonterminalTailD nontTail = new NonterminalTailD(nontHead, concatenation);

                        // add the tail to current children
                        concatenation.appendChild(nontTail);
                    }
                    else if (lexeme.Type == LexemeType.Terminal)
                    {
                        TerminalD terminalD = new TerminalD(lexeme.Value, concatenation);
                        concatenation.appendChild(terminalD);
                    }
                    else if (lexeme.Type == LexemeType.Undefined)
                    {
                        UndefinedD undefinedD = new UndefinedD(concatenation);
                        concatenation.appendChild(undefinedD);
                    }
                    else if (lexeme.Type != LexemeType.Concatenation)
                    {
                        // Lexes should only be of type Nont, T and Concat. Error
                        throw new Exception("Lexeme was not of type Nonterminal, Terminal or Concatenation");
                    }
                }

                current.appendChild(concatenation);
            }
        }

        /// <summary>
        /// Returns the nonterminal head with the name given. Null if not found.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private NonterminalHeadD getNonterminalHead(String name)
        {
            foreach (NonterminalHeadD nonterminalHead in rules)
            {
                if (nonterminalHead.Name.Equals(name))
                    return nonterminalHead;
            }

            // create the nonterminal
            NonterminalHeadD returnVal = new NonterminalHeadD(name);
            returnVal.Rule = new ChoiceLineD(returnVal);
            rules.Add(returnVal);
            synA.ProductionRulesHead.Add(new Lexeme(LexemeType.Nonterminal, name, -1));

            // create the rule tail
            List<Lexeme> newRule = new List<Lexeme>();
            newRule.Add(new Lexeme(LexemeType.Undefined, -1));
            synA.ProductionRulesTail.Add(newRule);
            

            return returnVal;
        }
    }
}
