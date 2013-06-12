using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.Global;

namespace VADG.DataStructure
{
    public class GrammarNodeDExistsVisitor : GrammarNodeD_Visitor
    {
        List<NonterminalHeadD> rulesVisited;
        
        private String name;
        private SymbolType symbolType;
        public GrammarNodeDExistsVisitor(String nameIn, SymbolType symbolTypeIn)
        {
            name = nameIn;
            symbolType = symbolTypeIn;
            rulesVisited = new List<NonterminalHeadD>();
            found = false;
        }

        private bool found;
        public bool Found
        {
            get { return found; }
        }


        
        public void visit(UndefinedD undefinedD)
        {
            if (symbolType == SymbolType.Undefined)
                found = true;
        }
        
        public void visit(TerminalD terminalD)
        {
            if (symbolType == SymbolType.Terminal)
            {
                if (terminalD.Name.Equals(name))
                    found = true;
            }
        }

        public void visit(NonterminalHeadD nonterminalHeadD)
        {
            rulesVisited.Add(nonterminalHeadD);
            
            if (symbolType == SymbolType.Nonterminal)
            {
                if (nonterminalHeadD.Name.Equals(name))
                {
                    found = true;
                    return;
                }
            }           
            

            // visit rule where representation is done for each choiceLineD
            nonterminalHeadD.Rule.accept(this);
        }

        public void visit(NonterminalTailD nonterminalTailD)
        {
            if (!found && !rulesVisited.Contains(nonterminalTailD.Reference))
                // visit the rule :)
                visit(nonterminalTailD.Reference);
        }

        public void visit(ConcatenationD concatenationD)
        {
            if (!found)
            {
                foreach (GrammarNodeD node in concatenationD.Children)
                    node.accept(this); 
            }
        }

        public void visit(ChoiceLineD choiceLineD)
        {
            if (!found)
            {
                foreach (GrammarNodeD node in choiceLineD.Children)            
                    node.accept(this); 
            } 
        }

        public void visit(ChoiceD choiceD)
        {
            if (!found)
            {
                foreach (GrammarNodeD node in choiceD.Children)
                    node.accept(this); 
            }
        }
    }
}
