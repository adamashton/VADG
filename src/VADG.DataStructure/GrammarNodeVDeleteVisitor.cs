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
    public class GrammarNodeVDeleteVisitor : GrammarNodeV_Visitor
    {

        private GrammarNodeD target;

        public GrammarNodeVDeleteVisitor(GrammarNodeD targetIn)
        {
            target = targetIn;
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
            int position = -1;
            
            for (int i = 0; i < concatenationV.Children.Count; i++)
            {
                GrammarNodeV child = concatenationV.Children[i];

                if (child.getReference() == target)
                {
                    position = i;
                    break;
                }

                child.accept(this);
            }

            if (position >= 0)
            {
                int theLevel = (concatenationV.Children[position] as SymbolV).Level;
                concatenationV.Children[position].Dispose();
                concatenationV.Children.RemoveAt(position);
                
                if (concatenationV.Children.Count == 0)
                {
                    // we add the undefined that should have been added by the Model.Function.Delete method

                    UndefinedD undefinedD = concatenationV.Reference.Children[0] as UndefinedD;

                    concatenationV.appendChildV(new UndefinedV(undefinedD, theLevel));
                }
            }
        }


        public void visit(ChoiceLineV choiceLineV)
        {
            foreach (GrammarNodeV child in choiceLineV.Children)
            {
                child.accept(this);
            }
        }

        public void visit(ChoiceV choiceV)
        {
            GrammarNodeV childToDelete = null;
            int position = -1;
            
            for (int i = 0; i < choiceV.Children.Count; i++)
            {
                GrammarNodeV child = choiceV.Children[i];

                if (child.getReference() == target)
                {
                    position = i;
                    break;
                }

                child.accept(this);

                // assert child is a concatenationV
                if (!(child is ConcatenationV))
                    throw new Exception("Child is not of type concatenationV");

                if ((child as ConcatenationV).Children.Count == 0)
                    childToDelete = child;                           
            }

            if (childToDelete != null)
            {
                childToDelete.Dispose();
                choiceV.Children.Remove(childToDelete);
            }

            if (position >= 0)
            {
                if (choiceV.Children.Count == 1)
                    throw new Exception("Should not be removing the choice when only one choice remains, sharpen up!");

                // much better, we're remvoing a choice
                choiceV.Children[position].Dispose();
                choiceV.Children.RemoveAt(position);
            }
        }
    }
}
