using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    /// <summary>
    /// A nonterminal head in the definitive grammar. Essentially a production rule.
    /// </summary>
    public class NonterminalHeadD : SymbolD
    {
        private ChoiceLineD rule;
        /// <summary>
        /// Defines the start of the rule which is always made up of a ChoiceLine.
        /// </summary>
        public ChoiceLineD Rule
        {
            get { return rule; }
            set { rule = value; }
        }
        
        private String name;
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        
        private List<NonterminalTailV> references;
        /// <summary>
        /// Where is this nonterminal used in the visualised grammar.
        /// </summary>        
        public List<NonterminalTailV> References
        {
            get { return references; }
            //set { references = value; }
        }

        private NonterminalHeadV refStartSymbol;
        public NonterminalHeadV RefStartSymbol
        {
            get { return refStartSymbol; }
            //set { refStartSymbol = value; }
        }

        public NonterminalHeadD(String nameIn)
        {
            Init(nameIn);
        }

        private void Init(string nameIn)
        {
            name = nameIn;
            references = new List<NonterminalTailV>();
        }
       
        #region Conversion methods

        /// <summary>
        /// Creates a new class that is a nonterminal head for use in the visualised grammar with all the references set up.
        /// </summary>
        /// <returns></returns>
        public NonterminalHeadV CreateNonterminalHeadV()
        {
            return new NonterminalHeadV(this);
        }

        /// <summary>
        /// Creates a new class that is a nonterminal tail for use in the definitive grammar, used to define a rule.
        /// </summary>
        /// <returns></returns>
        public NonterminalTailD CreateNonterminalTailD(GrammarNodeD parentIn)
        {
            return new NonterminalTailD(this, parentIn);
        }

        #endregion

        public string PrintRule()
        {
            if (rule == null || rule.Children == null || rule.Children.Count == 0)
                return name + " -> ?\n";
            
            String returnVal = "";
            foreach (OperationD operation in rule.Children)
                returnVal += name + " -> " + operation.ToString() + "\n";

            return returnVal;
        }
        
        //public NonterminalHeadV CreateVisualGrammar(Boolean updateRef)
        //{
        //    // Start creating a visual grammar from here
        //    NonterminalHeadV returnVal = new NonterminalHeadV(this);

        //    if (updateRef)
        //        refStartSymbol = returnVal;

            
        //    // traverse my child (always choice1)
        //    returnVal.Rule = (ChoiceLineV)rule.CreateVisual(updateRef);

        //    return returnVal;
        //}

        public override void accept(GrammarNodeD_Visitor visitor)
        {
            visitor.visit(this);
        }

        //internal override GrammarNodeV CreateVisual(bool updateRef)
        //{
        //    throw new NotImplementedException();
        //}

        public override void Dispose()
        {
            foreach (GrammarNodeV reference in references)
                reference.Dispose();

            if (refStartSymbol != null)
                refStartSymbol.Dispose();

            rule.Dispose();
        }

        /// <summary>
        /// Add a nonterminal tail v reference to the references of the definitive NonTHead
        /// </summary>
        /// <param name="refIn"></param>
        public void AddReference(NonterminalTailV refIn)
        {
            references.Add(refIn);
        }

        /// <summary>
        /// Set the reference if this nonterminal is the start symbol of the grammar
        /// </summary>
        /// <param name="refIn"></param>
        public void SetReference(NonterminalHeadV refIn)
        {
            refStartSymbol = refIn;
        }
    }
}
