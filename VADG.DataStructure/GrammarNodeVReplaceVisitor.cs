using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.DataStructure;
using System.Windows.Controls;
using System.Windows.Media;

namespace VADG.Drawer
{
    /// <summary>
    /// Adds a new UndefinedV item after every single
    /// </summary>
    public class GrammarNodeVReplaceVisitor : GrammarNodeV_Visitor
    {

        private int level = -1;
        private GrammarNodeD target;
        private GrammarNodeD newItem;
        
        public GrammarNodeVReplaceVisitor(GrammarNodeD targetIn, GrammarNodeD itemIn)
        {
            target = targetIn;
            newItem = itemIn;            
        }
        


        public void visit(UndefinedV undefinedV)
        {

        }

        public void visit(TerminalV terminalV)
        {

        }

        public void visit(NonterminalHeadV nonterminalHeadV)
        {
            // visit rule
            nonterminalHeadV.Rule.accept(this);
        }

        public void visit(NonterminalTailV nonterminalTailV)
        {
            if (nonterminalTailV.IsExpanded)
                nonterminalTailV.Rule.accept(this);
        }

        public void visit(ConcatenationV concatenationV)
        {
            for (int i = 0; i < concatenationV.Children.Count; i++)
            {
                GrammarNodeV child = concatenationV.Children[i];

                if (child.getReference() == target)
                {
                    level = (child as SymbolV).Level;
                    child.Dispose();
                    child = concatenationV.Children[i] = newVisual();                    
                }

                child.accept(this);                
            }            
        }


        public void visit(ChoiceLineV choiceLineV)
        {
            List<UserControl> children = new List<UserControl>();

            foreach (GrammarNodeV child in choiceLineV.Children)
            {
                child.accept(this);
            }
        }

        public void visit(ChoiceV choiceV)
        {

            foreach (GrammarNodeV child in choiceV.Children)
            {
                child.accept(this);
            }
        }

        private GrammarNodeV newVisual()
        {
            if (newItem is UndefinedD)
            {
                UndefinedD undefinedD = newItem as UndefinedD;
                UndefinedV visual = new UndefinedV(undefinedD, level);

                undefinedD.References.Add(visual);
                return visual;
            }
            else if (newItem is TerminalD)
            {
                TerminalD terminalD = newItem as TerminalD;
                TerminalV visual = new TerminalV(terminalD, level);

                terminalD.References.Add(visual);
                return visual;
            }
            else if (newItem is NonterminalTailD)
            {
                NonterminalTailD nontTailD = newItem as NonterminalTailD;
                NonterminalTailV visual = new NonterminalTailV(nontTailD, level);
                nontTailD.AddReference(visual);
                return visual;
            }
            else
                throw new NotImplementedException();

        }
    }
}
