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
    /// Draws the WPF components and puts the visualised grammar into components layered on each other
    /// </summary>
    public class GrammarNodeVDrawVisitor : GrammarNodeV_Visitor
    {
        private int level = 0;

        private UserControl drawnItem;
        public UserControl DrawnItem
        {
            get { return drawnItem; }
        }

        public void visit(UndefinedV undefinedV)
        {
            drawnItem = new Undefined(undefinedV);
        }

        public void visit(TerminalV terminalV)
        {
            drawnItem = new Terminal(terminalV);
        }

        public void visit(NonterminalHeadV nonterminalHeadV)
        {
            // visit rule
            nonterminalHeadV.Rule.accept(this);

            drawnItem = new NonterminalExpanded(nonterminalHeadV, drawnItem);
        }

        public void visit(NonterminalTailV nonterminalTailV)
        {
            if (nonterminalTailV.IsExpanded)
            {
                nonterminalTailV.Rule.accept(this);
                drawnItem = new NonterminalExpanded(nonterminalTailV, drawnItem);
            }
            else
            {
                drawnItem = new NonterminalCollapsed(nonterminalTailV);
            }
        }

        public void visit(ConcatenationV concatenationV)
        {
            List<UserControl> children = new List<UserControl>();

            foreach (GrammarNodeV child in concatenationV.Children)
            {
                child.accept(this);
                children.Add(drawnItem);
            }

            drawnItem = new Concatenation(children, concatenationV.Direction);
            children = null;
        }

        public void visit(ChoiceLineV choiceLineV)
        {
            List<UserControl> children = new List<UserControl>();

            foreach (GrammarNodeV child in choiceLineV.Children)
            {
                child.accept(this);
                children.Add(drawnItem);
            }

            drawnItem = new Concatenation(children, choiceLineV.Direction);
            children = null;
        }

        public void visit(ChoiceV choiceV)
        {
            List<UserControl> children = new List<UserControl>();

            foreach (GrammarNodeV child in choiceV.Children)
            {
                child.accept(this);
                children.Add(drawnItem);
            }

            drawnItem = new Choice(children, choiceV.Direction);
            children = null;
        }
    }
}
