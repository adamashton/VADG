using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public class ChoiceV : OperationV
    {
        #region Variables
        private ChoiceD reference;
        public ChoiceD Reference
        {
            get { return reference; }
        }
        public override GrammarNodeD getReference()
        {
            return reference;
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a visual node that has a reference to the definitive ChoiceD that was passed.
        /// </summary>
        /// <param name="ChoiceD"></param>
        public ChoiceV(ChoiceD refIn)
        {
            Init(new List<GrammarNodeV>(), refIn);
        }

        public ChoiceV(List<GrammarNodeV> children, ChoiceD refIn)
        {
            Init(children, refIn);
        }

        private void Init(List<GrammarNodeV> childrenIn, ChoiceD refIn)
        {
            reference = refIn;
            children = childrenIn;
        } 
        #endregion
        
        #region Overrides

        public override string ToString()
        {
            String returnVal = "";
            for (int i = 0; i < children.Count; i++)
            {
                returnVal += children[i].ToString();

                if (i != children.Count - 1)
                    returnVal += " | ";
            }

            return returnVal;
        }

        public override void Dispose()
        {
            // remove children
            foreach (GrammarNodeV child in children)
                child.Dispose();

            // remove reference from D
            reference.References.Remove(this);
        }

        public override void accept(GrammarNodeV_Visitor visitor)
        {
            visitor.visit(this);
        }

        public override GrammarNodeV getParent()
        {
            if (reference.Parent is ChoiceLineD)
            {
                ChoiceLineD parent = reference.Parent as ChoiceLineD;

                foreach (ChoiceLineV choiceLineV in parent.References)
                {
                    if (choiceLineV.Children.Contains(this))
                        return choiceLineV;
                }
            }
            
            throw new NotImplementedException();
        }
        #endregion
    }
}
