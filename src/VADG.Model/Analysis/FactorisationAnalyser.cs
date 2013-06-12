using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using VADG.DataStructure;
using VADG.Parser;

namespace VADG.Model.Analysis
{
    /// <summary>
    /// Will attempt to analyse a grammar and offer factorisations
    /// </summary>
    public static class FactorisationAnalyser
    {
        private static Hashtable hashtable;
        private static DefinitiveGrammar grammar;
        private static List<String> collisionKeys;

        /// <summary>
        /// Analyses the grammar and sets the possible factorisation variables
        /// Returns true iff a factorisation is possible.
        /// </summary>
        /// <returns></returns>
        public static List<Factorisation> Analyse(DefinitiveGrammar grammarIn)
        {
            // initialisation
            grammar = grammarIn;
            hashtable = new Hashtable();
            collisionKeys = new List<string>();

            // traverse grammar, adding all sub rules to the hashtable
            // and populating the collision keys
            PopulateHashTable();

            if (collisionKeys.Count > 0)
                return GetFactorisations();

            return null;
        }

        private static void PopulateHashTable()
        {
            foreach (NonterminalHeadD nontHead in grammar.Nonterminals)
            {                
                // get every subrule for this nonterminalHead
                foreach (SubRule subRule in SubRules(nontHead))
                {
                    String key = subRule.ToString();

                    if (hashtable.Contains(key))
                    {
                        // collision!
                        // append nonterminal to Factorisation
                        Factorisation hashValue = hashtable[key] as Factorisation;
                        if (!hashValue.NontHeads.Contains(nontHead))
                            hashValue.NontHeads.Add(nontHead);

                        // only add once for the collisions
                        if (!collisionKeys.Contains(key))
                            collisionKeys.Add(key);                        
                    }
                    else
                    {
                        hashtable.Add(key, new Factorisation(subRule, nontHead));
                    }
                }
            }
        }

        /// <summary>
        /// Returns all sub rules for a given rule in string representation
        /// </summary>
        /// <param name="nontHead"></param>
        /// <returns></returns>
        private static List<SubRule> SubRules(NonterminalHeadD nontHead)
        {
            // assert that choiceLineD has 1 child and it is of type ChoiceD
            if (nontHead.Rule.Children.Count > 1)
                throw new Exception("The choiceLineD has more than one child, cannot analyse.");

            if (!(nontHead.Rule.Children[0] is ChoiceD))
                throw new Exception("The rule does not have the structure ChoiceLineD -> ChoiceD -> ...");

            List<SubRule> returnVal = new List<SubRule>();

            // get the lexemes from the nonthead rule
            List<Lexeme> totalLexemes = LexemeGenerator.Reverse(nontHead.Rule.Children[0]);
            SubRule totalRule = new SubRule(totalLexemes, nontHead);

            for (int stepSize = 2; stepSize <= totalRule.SymbolCount; stepSize++)
            {
                for (int position = 0; position <= totalRule.SymbolCount - stepSize; position++)
                {
                    List<Lexeme> lexemes = totalRule.getSubRule(position, stepSize);
                    SubRule subRule = new SubRule(lexemes, nontHead);
                    returnVal.Add(subRule);
                }
            }

            return returnVal;
        }

        /// <summary>
        /// Returns the possible factorisations from the the list
        /// </summary>
        /// <returns>Null if no factorisatoin exists</returns>
        private static List<Factorisation> GetFactorisations()
        {
            if (collisionKeys == null)
                throw new Exception("You must analyse with a grammar first");

            if (collisionKeys.Count == 0)
                return null;

            int longest = -1;
            foreach (String key in collisionKeys)
            {
                int length = (hashtable[key] as Factorisation).SubRule.SymbolCount;
                if (length > longest)
                    longest = length;
            }

            List<Factorisation> returnVal = new List<Factorisation>();
            foreach (String key in collisionKeys)
            {
                if ((hashtable[key] as Factorisation).SubRule.SymbolCount >= longest)
                    returnVal.Add(hashtable[key] as Factorisation);
            }

            return returnVal;
        }
    }
}
