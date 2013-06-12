using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public class ConcatenationV : OperationV
    {
        #region Variables
        private ConcatenationD reference;
        public ConcatenationD Reference
        {
            get { return reference; }
        } 
        #endregion

        #region Constructors
        public ConcatenationV(ConcatenationD refIn)
        {
            Init(new List<GrammarNodeV>(), refIn);
        }

        public ConcatenationV(List<GrammarNodeV> children, ConcatenationD refIn)
        {
            Init(children, refIn);
        }

        private void Init(List<GrammarNodeV> childrenIn, ConcatenationD refIn)
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
                    returnVal += ", ";
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

        public override GrammarNodeD getReference()
        {
            return reference;
        }

        public override GrammarNodeV getParent()
        {
            if (reference.Parent is ChoiceD)
            {
                ChoiceD parent = reference.Parent as ChoiceD;

                foreach (ChoiceV choiceV in parent.References)
                {
                    if (choiceV.Children.Contains(this))
                        return choiceV;
                }                
            }

            throw new NotImplementedException();
        }
        #endregion
    }
}
