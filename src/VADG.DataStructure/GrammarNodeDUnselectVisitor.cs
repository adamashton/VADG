using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public class GrammarNodeDUnselectVisitor : GrammarNodeD_Visitor
    {
        public void visit(UndefinedD undefinedD)
        {
            undefinedD.IsSelected = false;
        }
        
        public void visit(TerminalD terminalD)
        {
            terminalD.IsSelected = false;
        }

        public void visit(NonterminalHeadD nonterminalHeadD)
        {
            nonterminalHeadD.IsSelected = false;

            // visit rule where representation is done for each choiceLineD
            nonterminalHeadD.Rule.accept(this);
        }

        public void visit(NonterminalTailD nonterminalTailD)
        {
            nonterminalTailD.IsSelected = false;
        }

        public void visit(ConcatenationD concatenationD)
        {
            foreach (GrammarNodeD node in concatenationD.Children)
                node.accept(this);
        }

        public void visit(ChoiceLineD choiceLineD)
        {
            foreach (GrammarNodeD node in choiceLineD.Children)            
                node.accept(this);
        }

        public void visit(ChoiceD choiceD)
        {
            foreach (GrammarNodeD node in choiceD.Children)
                node.accept(this);
        }
    }
}
