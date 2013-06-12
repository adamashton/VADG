using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    /// <summary>
    /// This class is found at the end of the tree of a rule. Simply a holder for when a rule is defined with a nonterminal within it.
    /// </summary>
    public class NonterminalTailD : SymbolD
    {
        
        private NonterminalHeadD reference;
        /// <summary>
        /// Pointer to what this nonterminal actually is in the defintive grammar.
        /// </summary>
        public NonterminalHeadD Reference
        {
            get { return reference; }
        }


        #region Constructors
        /// <summary>
        /// Creates a new class that has a reference to a nonterminal head D. For use in the definitive grammar when defining a rule.
        /// </summary>
        /// <param name="nonterminalHeadD"></param>
        public NonterminalTailD(NonterminalHeadD nonterminalHeadD, GrammarNodeD parentIn)
        {
            Init(nonterminalHeadD, parentIn);
        }

        private void Init(NonterminalHeadD nonterminalHeadD, GrammarNodeD parentIn)
        {
            reference = nonterminalHeadD;
            parent = parentIn;
        }

        #endregion

        #region Overrides        

        //internal override GrammarNodeV CreateVisual(Boolean updateRef)
        //{
        //    // create the nonterminal tail v to be used in the visualised grammar
        //    NonterminalTailV returnVal = new NonterminalTailV(this);

        //    if (updateRef)
        //        // add a pointer to the nonterminal V in the real nonterminals references
        //        reference.References.Add(returnVal);

        //    return returnVal;

        //}

        public override void accept(GrammarNodeD_Visitor visitor)
        {
            visitor.visit(this);
        }

        public override void Dispose()
        {
            parent = null;
        }

        public override string ToString()
        {
            return Reference.Name;
        }
        
        #endregion

        public void AddReference(NonterminalTailV refIn)
        {
            reference.AddReference(refIn);
        }
    }
}
