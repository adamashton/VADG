using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.DataStructure;
using System.Windows.Controls;
using System.Windows.Documents;

namespace VADG.Model
{
    public class DefinitiveGrammar
    {
        /// <summary>
        ///  which nonterminal is the starting symbol
        /// </summary>
        private NonterminalHeadD startSymbol;
        public NonterminalHeadD StartSymbol
        {
            get { return startSymbol; }           
        }

        /// <summary>
        /// The list of nonterminals that are found in the grammar
        /// </summary>
        private List<NonterminalHeadD> nonterminals;
        public List<NonterminalHeadD> Nonterminals
        {
            get { return nonterminals; }            
        }

        #region Constructors
        internal DefinitiveGrammar()
        {
            Init(new List<NonterminalHeadD>(), null);
        }

        internal DefinitiveGrammar(List<NonterminalHeadD> nonterminalsIn, NonterminalHeadD startSymbolIn)
        {
            Init(nonterminalsIn, startSymbolIn);
        }

        private void Init(List<NonterminalHeadD> nonterminalsIn, NonterminalHeadD startSymbolIn)
        {
            nonterminals = nonterminalsIn;
            startSymbol = startSymbolIn;
        } 
        #endregion

        /// <summary>
        /// Returns a nicely formatted grammar of the nonterminals followed by their rule
        /// </summary>
        /// <returns></returns>
        public String PrintGrammar()
        {
            GrammarNodeDTextualizeVisitor visitor = new GrammarNodeDTextualizeVisitor();
            
            String returnVal = "";
            foreach (NonterminalHeadD nont in nonterminals)
            {
                nont.accept(visitor);
                returnVal += visitor.Representation;
            }

            return returnVal;
        }

        public FlowDocument PrintGrammarRich()
        {
            // Create a FlowDocument to contain content for the RichTextBox.
            FlowDocument myFlowDoc = new FlowDocument();
            GrammarNodeDRichTextualizeVisitor visitor = new GrammarNodeDRichTextualizeVisitor();

            foreach (NonterminalHeadD nont in nonterminals)
            {
                nont.accept(visitor);
                myFlowDoc.Blocks.Add(visitor.Representation);
            }

            return myFlowDoc;
        }

        internal void setStartSymbol(NonterminalHeadD nonterminalHeadD)
        {
            if (nonterminals.Contains(nonterminalHeadD))
                startSymbol = nonterminalHeadD; 
            else
                throw new Exception("That start symbol is not in the grammar.");
        }

        public NonterminalHeadD getNonterminal(String name)
        {
            if (nonterminals == null)
                return null;

            foreach (NonterminalHeadD nonterminal in nonterminals)
            {
                if (nonterminal.Name.Equals(name))
                    return nonterminal;
            }

            return null;
        }

        internal List<string> getNonterminalNames()
        {
            List<String> returnVal = new List<string>();

            foreach (NonterminalHeadD nonterminal in nonterminals)
            {
                returnVal.Add(nonterminal.Name);
            }

            return returnVal;
        }
    }
}
