using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.Global;
using VADG.DataStructure;
using System.Windows.Documents;
using System.Windows;

namespace VADG.DataStructure
{
    /// <summary>
    /// creates the textualized grammar from the d grammar using the visitor pattern
    /// Prints bold if an item is selected
    /// </summary>
    public class GrammarNodeDRichTextualizeVisitor : GrammarNodeD_Visitor
    {
        private String concatenationOperator = Settings.ConcatOperator +  " ";
        private String choiceOperator = " " + Settings.ChoiceOperator + " ";
        private String ruleOperator = " " + Settings.AssignmentOperator + " ";
        private String choiceLineOperator = Environment.NewLine;
        private String endOfRuleOperator = Settings.EndOfRuleSymbol;
        private String terminalEncapsulation = Settings.TerminalEncapsulation;

        private NonterminalHeadD currentNonterminalHead; // used when writing the choiceLineD's

        private Paragraph representation = new Paragraph();
        public Paragraph Representation
        {
            get { return representation; }
        }


        public void visit(UndefinedD undefinedD)
        {
            String value = "???";

            Run run = new Run();

            run.Text = value;

            representation.Inlines.Add(run);
        }
        
        
        public void visit(TerminalD terminalD)
        {
            String value = terminalD.Name;

            Run run = new Run();
            if (terminalD.IsSelected)
                run.FontWeight = FontWeights.Bold;

            run.Text = terminalEncapsulation + value + terminalEncapsulation;

            representation.Inlines.Add(run);
        }



        public void visit(NonterminalHeadD nonterminalHeadD)
        {
            representation = new Paragraph();

            if (nonterminalHeadD.IsSelected)
                representation.FontWeight = FontWeights.Bold;

            currentNonterminalHead = nonterminalHeadD;

            // visit rule where representation is done for each choiceLineD
            nonterminalHeadD.Rule.accept(this);                 
        }

        public void visit(NonterminalTailD nonterminalTailD)
        {
            representation.Inlines.Add(new Run(nonterminalTailD.Reference.Name));
        }

        public void visit(ConcatenationD concatenationD)
        {
            foreach (GrammarNodeD node in concatenationD.Children)
            {
                node.accept(this);
                
                // if node isn't last node, print operator
                if (node != concatenationD.Children[concatenationD.Children.Count - 1])
                    representation.Inlines.Add(new Run(concatenationOperator));
            }
        }

        public void visit(ChoiceLineD choiceLineD)
        {
            foreach (GrammarNodeD node in choiceLineD.Children)
            {
                representation.Inlines.Add(new Run(currentNonterminalHead.Name + ruleOperator));

                node.accept(this);

                representation.Inlines.Add(new Run(endOfRuleOperator));
            }
        }

        public void visit(ChoiceD choiceD)
        {
            foreach (GrammarNodeD node in choiceD.Children)
            {
                node.accept(this);

                if (node != choiceD.Children[choiceD.Children.Count - 1])
                    representation.Inlines.Add(new Run(choiceOperator));
            }
        }
    }
}
