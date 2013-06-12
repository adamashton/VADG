using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    // creates the v grammar from the d grammar using the visitor pattern.
    public class GrammarNodeDCreateVisualVisitor : GrammarNodeD_Visitor
    {
        private int level;

        public GrammarNodeDCreateVisualVisitor()
        {
            level = 0;
        }

        public GrammarNodeDCreateVisualVisitor(int levelIn)
        {
            level = levelIn;
        }
        
        // the visualised node saved at current location
        private GrammarNodeV current;
        public GrammarNodeV CurrentNodeV
        {
            get { return current; }
        }


        public void visit(UndefinedD undefinedD)
        {
            UndefinedV undefinedV = new UndefinedV(undefinedD, level + 1);
            undefinedD.References.Add(undefinedV);

            current = undefinedV;
        }

        public void visit(TerminalD terminalD)
        {
            // set current, ready for parent
            TerminalV terminalV = new TerminalV(terminalD, level + 1);            

            terminalD.References.Add(terminalV);

            current = terminalV;
        }

        public void visit(NonterminalHeadD nonterminalHeadD)
        {
            //this is the start of the visit, we create a nonterminalheadV - note one in grammar
            // if this is called again then something has gone wrong
            if (current != null)
                throw new Exception("You should only be calling visit on a NonterminalHeadD node once when creating a visualised grammar. Also use the visitor pattern once, then re-initialize");

            // visit rule
            nonterminalHeadD.Rule.accept(this);
            // current should now be a ChoiceLineV with the rest of the visualised grammar

            NonterminalHeadV startSymbolV = nonterminalHeadD.CreateNonterminalHeadV();

            if (!(current is ChoiceLineV))
                throw new Exception("the current node is not of type ChoiceLineV, is the definitive grammar constructed properly?");

            startSymbolV.Rule = (ChoiceLineV)current;

            current = startSymbolV;
        }

        public void visit(NonterminalTailD nonterminalTailD)
        {
            NonterminalTailV nonterminalTailV = new NonterminalTailV(nonterminalTailD, level + 1);
            
            nonterminalTailD.AddReference(nonterminalTailV);

            current = nonterminalTailV;
        }

        public void visit(ConcatenationD concatenationD)
        {
            List<GrammarNodeV> children = new List<GrammarNodeV>();

            foreach (GrammarNodeD child in concatenationD.Children)
            {
                // make children
                child.accept(this);
                children.Add(current);                                
            }

            ConcatenationV concatenationV = new ConcatenationV(children, concatenationD);
            concatenationD.AddReference(concatenationV);

            current = concatenationV;
        }

        public void visit(ChoiceLineD choiceLineD)
        {
            List<GrammarNodeV> children = new List<GrammarNodeV>();

            foreach (GrammarNodeD child in choiceLineD.Children)
            {
                // make children
                child.accept(this);
                children.Add(current);
            }

            ChoiceLineV choiceLineV = new ChoiceLineV(children, choiceLineD);
            choiceLineD.AddReference(choiceLineV);

            current = choiceLineV;
        }

        public void visit(ChoiceD choiceD)
        {
            List<GrammarNodeV> children = new List<GrammarNodeV>();

            foreach (GrammarNodeD child in choiceD.Children)
            {
                // make children
                child.accept(this);
                children.Add(current);
            }

            ChoiceV choiceV = new ChoiceV(children, choiceD);
            choiceD.AddReference(choiceV);

            current = choiceV;
        }
    }
}
