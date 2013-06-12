using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public class ChoiceLineD : OperationD
    {
        #region Variables
        private List<ChoiceLineV> references;
        public List<ChoiceLineV> References
        {
            get { return references; }
        }
        #endregion

        #region Constructors
        public ChoiceLineD(GrammarNodeD parentIn)
        {
            Init(new List<GrammarNodeD>(), new List<ChoiceLineV>(), parentIn);
        }

        private void Init(List<GrammarNodeD> childrenIn, List<ChoiceLineV> referencesIn, GrammarNodeD parentIn)
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
                    returnVal += "\n";
            }

            return returnVal;
        }

        //internal override GrammarNodeV CreateVisual(Boolean updateRef)
        //{
        //    // create the visual node of Choice
        //    ChoiceLineV returnVal = new ChoiceLineV(this);

        //    if (updateRef)
        //        references.Add(returnVal);

        //    foreach (GrammarNodeD child in Children)
        //        returnVal.Children.Add(child.CreateVisual(updateRef));

        //    return returnVal;
        //}

        public override void accept(GrammarNodeD_Visitor visitor)
        {
            visitor.visit(this);
        }

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

        public void AddReference(ChoiceLineV choiceLineV)
        {
            references.Add(choiceLineV);
        }
    }
}
