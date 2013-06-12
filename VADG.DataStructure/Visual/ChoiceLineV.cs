using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public class ChoiceLineV : OperationV
    {
        #region Variables
        private ChoiceLineD reference;
        public ChoiceLineD Reference
        {
            get { return reference; }
        }
        #endregion

        #region Constructors
        public ChoiceLineV(ChoiceLineD refIn)
        {
            Init(new List<GrammarNodeV>(), refIn);
        }

        public ChoiceLineV(List<GrammarNodeV> children, ChoiceLineD refIn)
        {
            Init(children, refIn);
        }

        private void Init(List<GrammarNodeV> childrenIn, ChoiceLineD refIn)
        {
            children = childrenIn;
            reference = refIn;
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
                    returnVal += "\n  is ";
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
            throw new NotImplementedException();
        }
       
        #endregion
    }
}
