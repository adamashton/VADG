using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.Parser;
using VADG.DataStructure;
using VADG.Model.Analysis;

namespace VADG.Model.Analysis
{
    public class Factorisation
    {
        private SubRule subRule;

        public SubRule SubRule
        {
            get { return subRule; }
            set { subRule = value; }
        }
        private List<NonterminalHeadD> nontHeads;

        public List<NonterminalHeadD> NontHeads
        {
            get { return nontHeads; }
            set { nontHeads = value; }
        }

        public Factorisation(SubRule subRuleIn, List<NonterminalHeadD> nontHeadsIn)
        {
            subRule = subRuleIn;
            nontHeads = nontHeadsIn;
        }

        public Factorisation(SubRule subRuleIn, NonterminalHeadD nontHeadIn)
        {
            subRule = subRuleIn;
            nontHeads = new List<NonterminalHeadD>();
            nontHeads.Add(nontHeadIn);
        }

        public override string ToString()
        {
            String returnVal = "Factorisation on rules: ";

            for (int i = 0; i < nontHeads.Count; i++)
            {
                returnVal += nontHeads[i].Name;

                if (i != nontHeads.Count - 1)
                    returnVal += ", ";
            }

            return returnVal;
        }

        public String applyRule(String grammarText, String ruleName)
        {
            // assert grammar contrains the factorisation sub rule
            if (grammarText.Contains(subRule.ToString()))
            {
                String returnVal = grammarText.Replace(subRule.ToString(), ruleName);
                returnVal += "\n" + ruleName + VADG.Global.Settings.AssignmentOperator + subRule.ToString() + VADG.Global.Settings.EndOfRuleSymbol;

                return returnVal;
            }
            
            throw new Exception("The Subrule was not found in the grammar, cannot apply factorisation");
        }
    }
}
