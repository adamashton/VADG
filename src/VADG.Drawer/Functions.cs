using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.DataStructure;
using System.Windows.Controls;

namespace VADG.Drawer
{
    /// <summary>
    /// An interface for performing functions on the view
    /// </summary>
    public class Functions
    {
        private NonterminalHeadV startSymbol;

        public Functions(NonterminalHeadV startSymbolIn)
        {
            startSymbol = startSymbolIn;
        }
        
        /// <summary>
        /// Expands the view, assumes that the visualised grammar has been expanded
        /// </summary>
        /// <param name="control"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        public void Expand(NonterminalCollapsed control)
        {
            StackPanel parent = (control.Parent as StackPanel);
            if (parent == null)
                throw new Exception("The controls parent is not of type stack panel");

            // draw the control
            GrammarNodeVDrawVisitor visitor = new GrammarNodeVDrawVisitor();
            visitor.visit(control.Reference);

            // get the index of the nonterminal collapsed that wants to expand
            int index = parent.Children.IndexOf(control);
            if (index < 0)
                throw new Exception("Cant find child in my stack panel, uh oh!");

            parent.Children.RemoveAt(index);
            parent.Children.Insert(index, visitor.DrawnItem);
        }

        public void Collapse(NonterminalExpanded control)
        {
            StackPanel parent = (control.Parent as StackPanel);
            if (parent == null)
                throw new Exception("The controls parent is not of type stack panel");

            // draw collapsed control
            GrammarNodeVDrawVisitor visitor = new GrammarNodeVDrawVisitor();
            visitor.visit(control.ReferenceTail);

            // get the index of the nonterminal collapsed that wants to expand
            int index = parent.Children.IndexOf(control);
            if (index < 0)
                throw new Exception("Cant find child in my stack panel, uh oh!");

            parent.Children.RemoveAt(index);
            parent.Children.Insert(index, visitor.DrawnItem);
        }

        public void Rename(Terminal terminalControl, string newName)
        {
            ViewController.ViewEvents.Rename(terminalControl, newName);
        }

        public void Select(Terminal terminalControl)
        {
            ViewController.ViewEvents.Select(terminalControl);
        }

        public void AddAfter(SymbolV symbolV, GrammarNodeD itemToAdd)
        {
            GrammarNodeVAddAfterVisitor visitor = new GrammarNodeVAddAfterVisitor(symbolV, itemToAdd);
            
            startSymbol.accept(visitor);
        }

        public void AddBefore(SymbolV symbolV, GrammarNodeD itemToAdd)
        {
            GrammarNodeVAddBeforeVisitor visitor = new GrammarNodeVAddBeforeVisitor(symbolV, itemToAdd);
            
            startSymbol.accept(visitor);
        }


        public void Replace(GrammarNodeD itemToReplace, GrammarNodeD newItem)
        {
            GrammarNodeVReplaceVisitor visitor = new GrammarNodeVReplaceVisitor(itemToReplace, newItem);            

            startSymbol.accept(visitor);
        }

        public void Delete(GrammarNodeV itemToDelete)
        {
            GrammarNodeVDeleteVisitor visitor = new GrammarNodeVDeleteVisitor(itemToDelete.getReference());
            
            startSymbol.accept(visitor);
        }

        public void AddChoice(SymbolV symbolV)
        {
            GrammarNodeVAddChoiceVisitor visitor = new GrammarNodeVAddChoiceVisitor(symbolV.getReference());

            startSymbol.accept(visitor);
        }


        public void Delete(ConcatenationD concatD)
        {
            GrammarNodeVDeleteVisitor visitor = new GrammarNodeVDeleteVisitor(concatD);
            startSymbol.accept(visitor);
        }
    }
}
