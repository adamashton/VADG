using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    /// <summary>
    /// The start symbol of the visualised grammar. Only item to point to a head nont. Program will have one instance for each visualisation.
    /// </summary>
    public class NonterminalHeadV : SymbolV
    {
        private ChoiceLineV rule;
        /// <summary>
        /// Defines the start of the rule which is always made up of a ChoiceLine.
        /// </summary>
        public ChoiceLineV Rule
        {
            get { return rule; }
            set { rule = value; }
        }
        
        private NonterminalHeadD reference;
        /// <summary>
        /// What it is actually in the definitive grammar.
        /// </summary>
        public NonterminalHeadD Reference
        {
            get { return reference; }
        }

        /// <summary>
        /// Reference to Name in NonterminalHeadD
        /// </summary>
        public String Name
        {
            get { return Reference.Name; }
        }

        #region Constructors

        /// <summary>
        /// Creates a new nonterminal head for use in the visuualised grammar, with references to it's nonterminal head D passed.
        /// </summary>
        /// <param name="nonterminalHeadD"></param>
        public NonterminalHeadV(NonterminalHeadD nonterminalHeadD)
        {
            Init(nonterminalHeadD);            
        }

        private void Init(NonterminalHeadD nonterminalHeadD)
        {
            reference = nonterminalHeadD;
        }

        #endregion

        #region Overrides
        public override string ToString()
        {
            return "Start of Grammar\n" + reference.Name + " is " + rule.ToString();
        }


        public override void Dispose()
        {
            rule.Dispose();

            reference.SetReference(null);
        }

        public override GrammarNodeD getReference()
        {
            return reference;
        }

        /// <summary>
        /// Will traverse visualised grammar, expanding the list of nonterminals just once
        /// </summary>
        //public void ExpandAllNonterminalsOnce()
        //{
        //    List<NonterminalHeadD> seen = new List<NonterminalHeadD>();
        //    seen.Add(reference);
        //    rule.ExpandAllNonterminalsOnce(seen);
        //}

        //internal override void ExpandAllNonterminalsOnce(List<NonterminalHeadD> seen)
        //{
        //    // should never be called as headv is always expanded
        //    throw new NotImplementedException();
        //}

        public override void accept(GrammarNodeV_Visitor visitor)
        {
            visitor.visit(this);
        }

        
        #endregion
    }
}
