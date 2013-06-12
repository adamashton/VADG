using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public abstract class OperationV : GrammarNodeV
    {
        protected List<GrammarNodeV> children;
        /// <summary>
        /// This Operation will always have at least one child, can be nonterminals or more operations
        /// </summary>
        public List<GrammarNodeV> Children
        {
            get { return children; }
        }

        public void appendChildV(GrammarNodeV child)
        {
            if (child is NonterminalHeadV)
                throw new Exception("An operations children must not be of type NonterminalHeadV.");

            children.Add(child);
        }

        public void insertChildV(int position, GrammarNodeV child)
        {
            if (child is NonterminalHeadV)
                throw new Exception("An operations children must not be of type NonterminalHeadV.");

            if (position < 0)
                throw new Exception("The position was less than zero, cmon, sharpen up!");

            if (position > children.Count)
                throw new Exception("The position was greater than the children array");

            // do it!
            if (position == children.Count)
                appendChildV(child);
            else
                children.Insert(position, child);

        }

        //internal override void ExpandAllNonterminalsOnce(List<NonterminalHeadD> seen)
        //{
        //    foreach (GrammarNodeV child in children)
        //        child.ExpandAllNonterminalsOnce(seen);
        //}

        public abstract GrammarNodeV getParent();        
    }
}
