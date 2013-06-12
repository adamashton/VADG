using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.DataStructure;

namespace VADG.Model
{
    public class VisualisedGrammar
    {
        private NonterminalHeadV startSymbol;
        /// <summary>
        /// Start of visualised grammar.
        /// </summary>
        public NonterminalHeadV StartSymbol
        {
            get { return startSymbol; }
            set { startSymbol = value; }
        }

        #region Constructors
        private void Init(NonterminalHeadV startSymbolIn)
        {
            startSymbol = startSymbolIn;
        }

        /// <summary>
        /// Creates a visualised grammar based on the definitive grammar passed.
        /// </summary>
        /// <param name="dGrammarIn"></param>
        internal VisualisedGrammar(DefinitiveGrammar dGrammarIn)
        {
            // obsolete - using vistor pattern
            //startSymbol = dGrammarIn.StartSymbol.CreateVisualGrammar(true);

            // create visitor
            GrammarNodeDCreateVisualVisitor visitor = new GrammarNodeDCreateVisualVisitor();
            // traverse tree
            dGrammarIn.StartSymbol.accept(visitor);

            if (visitor.CurrentNodeV as NonterminalHeadV == null)
                throw new Exception("Creating the visualised grammar from the definitive failed.");

            startSymbol = (NonterminalHeadV)visitor.CurrentNodeV;
        }
        #endregion

        public String PrintGrammar()
        {
            return startSymbol.ToString();
        }

        public void accept(GrammarNodeV_Visitor visitor)
        {
            startSymbol.accept(visitor);
        }
    }
}
