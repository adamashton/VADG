using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public class ConcatenationD : OperationD
    {
        #region Variables
        private List<ConcatenationV> references;
        public List<ConcatenationV> References
        {
            get { return references; }
        }
        #endregion

        #region Constructors
        public ConcatenationD(GrammarNodeD parentIn)
        {
            Init(new List<GrammarNodeD>(), new List<ConcatenationV>(), parentIn);
        }

        private void Init(List<GrammarNodeD> childrenIn, List<ConcatenationV> referencesIn, GrammarNodeD parentIn)
        {
            children = childrenIn;
            references = referencesIn;
            parent = parentIn;
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
                    returnVal += ",";
            }

            return returnVal;
        }

        //internal override GrammarNodeV CreateVisual(Boolean updateRef)
        //{
        //    // create the visual node 
        //    ConcatenationV returnVal = new ConcatenationV(this);

        //    if (updateRef)
        //        references.Add(returnVal);

        //    // as a concatenatin should have at least 1 child, add 
        //    foreach (GrammarNodeD child in Children)
        //    {
        //        GrammarNodeV vNode = child.CreateVisual(updateRef);
        //        returnVal.Children.Add(vNode);
        //    }

        //    return returnVal;
        //}

        public void AddReference(ConcatenationV refIn)
        {
            references.Add(refIn);
        }

        public override void accept(GrammarNodeD_Visitor visitor)
        {
            visitor.visit(this);
        }

        //public ConcatenationV CreateVisual()
        //{
        //    return CreateVisual(true) as ConcatenationV;
        //}

        public override void Dispose()
        {
            parent = null;

            for (int i = 0; i < references.Count; i++)
                references[i].Dispose();

            references.Clear();

            for (int i = 0; i < children.Count; i++)
                children[i].Dispose();

            children.Clear();
        }
        #endregion
    }
}
