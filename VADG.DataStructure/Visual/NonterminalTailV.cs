using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    /// <summary>
    /// Defines a nonterminal in the visualised grammar. iff expanded will have a rule defined with it.
    /// </summary>
    public class NonterminalTailV : SymbolV
    {
        #region Variables
        private ChoiceLineV rule;
        public ChoiceLineV Rule
        {
            get { return rule; }
            set { rule = value; }
        }

        private NonterminalTailD reference;
        public NonterminalTailD Reference
        {
            get { return reference; }
        }

        
        #endregion

        #region Constructors
        public NonterminalTailV(NonterminalTailD refIn, int levelIn)
        {
            Init(refIn, levelIn);
        }

        private void Init(NonterminalTailD refIn, int levelIn)
        {
            reference = refIn;
            level = levelIn;
        } 
        #endregion

        #region Overrides
        public override string ToString()
        {
            if (rule == null)
                return reference.ToString();

            return "[" + reference.ToString() + " is " + rule.ToString() + "]";

        }

        public override GrammarNodeD getReference()
        {
            return reference;
        }
        
        #endregion

        /// <summary>
        /// Reference to Name in NonterminalHeadD
        /// </summary>
        public String Name
        {
            get { return Reference.Reference.Name; }
        }

        public Boolean IsExpanded
        {
            get { return rule != null; }
        }

        public void Expand()
        {
            if (IsExpanded)
                throw new Exception("Cannot expand an expanded nonterminal tail V.");

            GrammarNodeDCreateVisualVisitor visitor = new GrammarNodeDCreateVisualVisitor(level);
            Reference.Reference.Rule.accept(visitor);

            if (visitor.CurrentNodeV as ChoiceLineV == null)
                throw new Exception("The node created in the visitor pattern returned null or is not of type ChoiceLineV");
            else
                rule = (ChoiceLineV)visitor.CurrentNodeV;
            //rule = (ChoiceLineV)reference.Reference.Rule.CreateVisual(updateRef)   
        }

        public void Collapse()
        {
            if (!IsExpanded)
                throw new Exception("Cannot collapse a collapsed nonterminal tail V.");

            rule.Dispose();

            rule = null;

            // reset to default. Need to use visitor pattern to create visual from definitive, see log book.
            Direction = false;
        }

        public override void Dispose()
        {
            if (IsExpanded)
                // remove children
                rule.Dispose();

            // remove reference from NonterminalHeadD            
            reference.Reference.References.Remove(this);
        }

        //internal override void ExpandAllNonterminalsOnce(List<NonterminalHeadD> seen)
        //{
        //    if (seen.Contains(this.Reference.Reference))
        //        // do nothing
        //        return;

        //    seen.Add(this.Reference.Reference);

        //    // add rule
        //    rule = (ChoiceLineV)Reference.Reference.Rule.CreateVisual(true);

        //    rule.ExpandAllNonterminalsOnce(seen);
        //}

        public override void accept(GrammarNodeV_Visitor visitor)
        {
            visitor.visit(this);
        }

       
    }
}
