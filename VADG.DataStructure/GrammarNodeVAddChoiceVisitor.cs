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
    /// Adds a choice into the choiceD that is parent parent of the symbolV
    /// Adds an undefinedV there
    /// </summary>
    public class GrammarNodeVAddChoiceVisitor : GrammarNodeV_Visitor
    {

        private GrammarNodeD target;

        public GrammarNodeVAddChoiceVisitor(GrammarNodeD targetIn)
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
                    position = i;
                
                child.accept(this);
            }

            if (position >= 0)
            {
                int theLevel = (concatenationV.Children[position] as SymbolV).Level;

                ChoiceV choiceV = concatenationV.getParent() as ChoiceV;
                if (choiceV == null)
                    throw new Exception("The concatV parent was not of type choiceV");

                // assert that a new choice has been added to the concatD
                ChoiceD choiceD = concatenationV.Reference.Parent as ChoiceD;
                if (choiceD == null)
                    throw new Exception("The concatV.Ref.Parent is not of type choiceD");

                if (choiceD.Children.Count < 2)
                    throw new Exception("A new choice has not been added to the choiceD grammar.");

                // assuming that the last choice to be added is the new one.
                ConcatenationD concatDadded = choiceD.Children[choiceD.Children.Count - 1] as ConcatenationD;
                if (concatDadded == null)
                    throw new Exception("The choiceD children are not of type concatD");

                // assert that the new concatD has one child of type UndefinedD
                if (concatDadded.Children.Count > 1)
                    throw new Exception("ConcatD is new and does not have one child of type UndefinedD");

                UndefinedD undefinedD = concatDadded.Children[0] as UndefinedD;
                if (undefinedD == null)
                    throw new Exception("ConcatD is new and does not have one child of type UndefinedD");

                ConcatenationV newConcat = new ConcatenationV(concatDadded);
                UndefinedV newUndefinedV = new UndefinedV(undefinedD, theLevel);

                newConcat.appendChildV(newUndefinedV);
                choiceV.appendChildV(newConcat);
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
            int initialCount = choiceV.Children.Count;
            for (int i = 0; i < initialCount; i++)
            {
                choiceV.Children[i].accept(this);
            }
        }
    }
}
