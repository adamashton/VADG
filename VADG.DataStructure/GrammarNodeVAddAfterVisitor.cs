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
    public class GrammarNodeVAddAfterVisitor : GrammarNodeV_Visitor
    {
        private GrammarNodeV target;
        private GrammarNodeD item;
        
        public GrammarNodeVAddAfterVisitor(GrammarNodeV targetIn, GrammarNodeD itemIn)
        {
            target = targetIn;
            item = itemIn;
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
            int i = 0;
            int position = -1;
            foreach (GrammarNodeV child in concatenationV.Children)
            {
                
                if (child.getReference() == target.getReference())
                    position = i + 1;
                                       
                child.accept(this);
                i++;
            }

            if (position > -1)
            {
                int curLevel = (concatenationV.Children[0] as SymbolV).Level;
                concatenationV.insertChildV(position, newVisual(curLevel));
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

        private GrammarNodeV newVisual(int level)
        {
            if (item is UndefinedD)
            {
                UndefinedD undefinedD = item as UndefinedD;
                UndefinedV visual = new UndefinedV(undefinedD, level);
                
                undefinedD.References.Add(visual);
                return visual;
            }
            
            
            throw new NotImplementedException();
        }
    }
}
