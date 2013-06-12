using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public abstract class OperationD : GrammarNodeD
    {
        protected List<GrammarNodeD> children;
        /// <summary>
        /// This Operation will always have at least one child, can be nonterminals or more operations
        /// </summary>
        public List<GrammarNodeD> Children
        {
            get { return children; }
        }

        public void appendChild(GrammarNodeD child)
        {
            if (child is NonterminalHeadD)
                throw new Exception("An operations children must be of type NonterminalTailTailD.");            

            children.Add(child);
        }

        /// <summary>
        /// Will put child at the position in the children
        /// If position is at end will append to end
        /// </summary>
        /// <param name="position"></param>
        public void insertChild(int position, GrammarNodeD child)
        {
            if (position > children.Count || children.Count < 0)
                throw new Exception("The position attempted [" + position + "] was outside the bounds of the children array");

            if (position == children.Count)
                appendChild(child);
            else
                children.Insert(position, child);
        }
    }
}
