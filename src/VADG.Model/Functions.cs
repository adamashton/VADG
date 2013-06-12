using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.DataStructure;

namespace VADG.Model
{
    /// <summary>
    ///  the set of functions for the model
    /// </summary>
    public class Functions
    {
        // the model associated with this Function class
        GrammarModel model;

        public Functions(GrammarModel modelIn)
        {
            model = modelIn;
        }
        
        public void Expand(NonterminalTailV control)
        {
            control.Expand();                            
        }

        public void Collapse(NonterminalTailV control)
        {
            control.Collapse();
        }


        public void Rename(TerminalV terminalV, string newName)
        {
            terminalV.Reference.Name = newName;
        }

        public void Rename(NonterminalHeadD nont, string newName)
        {
            nont.Name = newName;
        }

        
        public GrammarNodeD AddAfter(NonterminalHeadD nonterminalHeadD, GrammarNodeV target, VADG.Global.SymbolType symbolType)
        {
            GrammarNodeDAddAfterVisitor visitor = new GrammarNodeDAddAfterVisitor();
            visitor.Target = target.getReference();
            visitor.SymbolType = symbolType;

            visitor.visit(nonterminalHeadD);

            return visitor.Item;
        }

        public GrammarNodeD AddBefore(NonterminalHeadD nonterminalHeadD, GrammarNodeV grammarNodeV, VADG.Global.SymbolType symbolType)
        {
            GrammarNodeDAddBeforeVisitor visitor = new GrammarNodeDAddBeforeVisitor();
            visitor.Target = grammarNodeV.getReference();
            visitor.SymbolType = symbolType;

            visitor.visit(nonterminalHeadD);

            return visitor.Item;
        }

        public GrammarNodeD Replace(GrammarNodeD itemToReplace, VADG.Global.SymbolType symbolType, string name)
        {
            OperationD parent = itemToReplace.Parent as OperationD;
            if (parent == null)
                throw new Exception("The item to replace does not have an operation D as a parent, fail.");

            // the rule we are editing
            NonterminalHeadD rule = itemToReplace.getMyRule();
            if (rule == null)
                throw new Exception("The item to replace could not find a nonterminalhead D associated with it.");

            // remove old item
            int index = parent.Children.IndexOf(itemToReplace);
            parent.Children.Remove(itemToReplace);
            itemToReplace.Dispose();

            // create new item to add
            GrammarNodeD newItem;
            if (symbolType == VADG.Global.SymbolType.Terminal)
            {
                // create new Terminal
                newItem = new TerminalD(name, parent);
            }
            else
            {
                // If the nont head doesn't exist, create and add to grammar
                NonterminalHeadD nontHead = model.DefinitiveGrammar.getNonterminal(name);

                if (nontHead == null)
                    nontHead = CreateNewNonterminal(name);

                newItem = nontHead.CreateNonterminalTailD(parent);
            }

            // insert new item into parent
            parent.insertChild(index, newItem);
            return newItem;
        }

        /// <summary>
        /// Creates a new nonterminal and add's it to the defintive grammar. 
        /// It's rule is a simple Nont -> ChoiceLineD -> ConcatD -> Undefined
        /// </summary>
        /// <param name="name"></param>
        public NonterminalHeadD CreateNewNonterminal(String name)
        {
            if (model.DefinitiveGrammar.getNonterminal(name) != null)
                throw new Exception("The given name already exists as a nonterminal, dummy");

            // build nonterminal
            NonterminalHeadD newNonterminal = new NonterminalHeadD(name);
            ChoiceLineD choiceLineD = new ChoiceLineD(newNonterminal);
            newNonterminal.Rule = choiceLineD;

            ChoiceD choiceD = new ChoiceD(choiceLineD);
            newNonterminal.Rule.appendChild(choiceD);
            
            ConcatenationD concatD = new ConcatenationD(choiceD);
            choiceD.appendChild(concatD);

            concatD.appendChild(new UndefinedD(concatD));

            // add to model
            model.DefinitiveGrammar.Nonterminals.Add(newNonterminal);

            return newNonterminal;
        }



        public void Delete(GrammarNodeD grammarNodeD)
        {
            if (grammarNodeD.Parent is ConcatenationD)
            {
                ConcatenationD parent = grammarNodeD.Parent as ConcatenationD;               

                // do the removal
                grammarNodeD.Dispose();
                parent.Children.Remove(grammarNodeD);

                if (parent.Children.Count == 0)
                    // add undefined
                    parent.appendChild(new UndefinedD(parent));                
            }
            else if (grammarNodeD.Parent is ChoiceD)
            {
                // removing a choice
                ChoiceD parent = grammarNodeD.Parent as ChoiceD;

                // do removal
                grammarNodeD.Dispose();
                parent.Children.Remove(grammarNodeD);

                if (parent.Children.Count == 0)
                {
                    ConcatenationD concatD = new ConcatenationD(parent);
                    parent.appendChild(concatD);
                    concatD.appendChild(new UndefinedD(concatD));
                }
            }
            else
                throw new Exception("The parent was not of type concatenation, I don't know what to do...");
        }

        public void AddChoice(GrammarNodeD grammarNodeD)
        {
            // assert parent is concatenation
            ConcatenationD parent = grammarNodeD.Parent as ConcatenationD;
            if (parent == null)
                throw new Exception("The grammarNodeD parent is not of type concatenation");

            // assert parent parent is choiceD
            ChoiceD parentParent = parent.Parent as ChoiceD;
            if (parentParent == null)
                throw new Exception("The grammarNodeD parent parent is not of type choiceD");

            ConcatenationD concatD = new ConcatenationD(parentParent);
            parentParent.appendChild(concatD);

            concatD.appendChild(new UndefinedD(concatD));            
        }

        public void DeleteRule(NonterminalHeadD rule)
        {
            model.DefinitiveGrammar.Nonterminals.Remove(rule);
        }
    }
}
