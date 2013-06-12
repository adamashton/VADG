using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public abstract class GrammarNodeD
    {
        //internal abstract GrammarNodeV CreateVisual(Boolean updateRef);

        public abstract void Dispose();

        public abstract void accept(GrammarNodeD_Visitor visitor);

        private bool isSelected = false;
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        /// <summary>
        /// Returns the nonterminalHeadD for the production rule this item is in.
        /// </summary>
        /// <returns></returns>
        public NonterminalHeadD getMyRule()
        {
            // if a control doesn't have a parent it is assumed to be the nonterminalheadd
            if (parent != null)
                return parent.getMyRule();
            else
            {
                if (this is NonterminalHeadD)
                    return (this as NonterminalHeadD);
                else
                    throw new Exception("This grammar node D parent is null. Only NontermnalHeadD have null parents.");
            }
        }

        /// <summary>
        /// A reference to the parent node d in the grammar
        /// </summary>
        protected GrammarNodeD parent;
        public GrammarNodeD Parent
        {
            get { return parent; }
        }
    }
}
