using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.Global;

namespace VADG.DataStructure
{
    // creates the textualized grammar from the d grammar using the visitor pattern
    public class GrammarNodeDTextualizeVisitor : GrammarNodeD_Visitor
    {
        private String concatenationOperator = Settings.ConcatOperator +  " ";
        private String choiceOperator = " " + Settings.ChoiceOperator + " ";
        private String ruleOperator = " " + Settings.AssignmentOperator + " ";
        private String choiceLineOperator = Environment.NewLine;
        private String endOfRuleOperator = Settings.EndOfRuleSymbol;
        private String terminalEncapsulation = Settings.TerminalEncapsulation;

        private NonterminalHeadD currentNonterminalHead; // used when writing the choiceLineD's

        private String representation = "";
        public String Representation
        {
            get { return representation; }
        }

        public void visit(UndefinedD undefinedD)
        {
            String value = "???";

            representation += value;
        }

        public void visit(TerminalD terminalD)
        {
            String value = terminalD.Name;

            representation += terminalEncapsulation + value + terminalEncapsulation;            
        }

        public void visit(NonterminalHeadD nonterminalHeadD)
        {
            representation = "";
            currentNonterminalHead = nonterminalHeadD;

            // visit rule where representation is done for each choiceLineD
            nonterminalHeadD.Rule.accept(this);                 
        }

        public void visit(NonterminalTailD nonterminalTailD)
        {
            representation += nonterminalTailD.Reference.Name;
        }

        public void visit(ConcatenationD concatenationD)
        {
            foreach (GrammarNodeD node in concatenationD.Children)
            {
                node.accept(this);
                
                // if node isn't last node, print operator
                if (node != concatenationD.Children[concatenationD.Children.Count - 1])                    
                    representation += concatenationOperator;                
            }
        }

        public void visit(ChoiceLineD choiceLineD)
        {
            foreach (GrammarNodeD node in choiceLineD.Children)
            {
                representation += currentNonterminalHead.Name + ruleOperator;

                node.accept(this);

                representation += endOfRuleOperator + choiceLineOperator;
            }
        }

        public void visit(ChoiceD choiceD)
        {
            foreach (GrammarNodeD node in choiceD.Children)
            {
                node.accept(this);

                if (node != choiceD.Children[choiceD.Children.Count - 1])
                    representation += choiceOperator;
            }
        }
    }
}
