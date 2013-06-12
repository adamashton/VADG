using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.DataStructure;
using VADG.Parser;

namespace VADG.Model
{
    /// <summary>
    /// The main interface to the grammar data structures. The Model in the MVC.
    /// </summary>
    public class GrammarModel
    {
        #region Variables
        private VisualisedGrammar visualisedGrammar;
        public VisualisedGrammar VisualisedGrammar
        {
            get
            {
                if (visualisedGrammar == null)
                    buildVisualisedGrammar();

                return visualisedGrammar;
            }
        }

        private DefinitiveGrammar definitiveGrammar;
        public DefinitiveGrammar DefinitiveGrammar
        {
            get { return definitiveGrammar; }
        }

        private Functions functions;
        public Functions Functions
        {
            get
            {
                if (functions == null)
                    functions = new Functions(this);
                return functions;
            }
        }

        BNFParser bnfParser;

        #endregion

        #region Constructors
        public GrammarModel()
        {
            bnfParser = new BNFParser();
        }

        public GrammarModel(DefinitiveGrammar definitiveGrammarIn)
        {
            bnfParser = new BNFParser();
            definitiveGrammar = definitiveGrammarIn;
        }

        public GrammarModel(String grammar)
        {
            bnfParser = new BNFParser();
            setDefinitiveGrammar(grammar);
        }


        #endregion

        private void buildVisualisedGrammar()
        {
            if (definitiveGrammar == null)
                throw new Exception("You must set the definitive grammar first");

            visualisedGrammar = new VisualisedGrammar(definitiveGrammar);
        }

        public void setDefinitiveGrammar(DefinitiveGrammar dg)
        {
            definitiveGrammar = dg;
            visualisedGrammar = null;
        }

        /// <summary>
        /// Will construct a definitive grammar from an input string, throw exception with compiler error otherwise.
        /// Assumes first rule is the start symbol.
        /// </summary>
        /// <param name="input"></param>
        public void setDefinitiveGrammar(String input)
        {
            bool noError = bnfParser.Parse(input);
            if (noError)
            {
                // no start symbol specified, assume first entry
                definitiveGrammar = new DefinitiveGrammar(bnfParser.Rules, bnfParser.Rules[0]);
                visualisedGrammar = null;
            }
            else
            {
                throw new Exception(bnfParser.ErrorText);
            }
        }

        /// <summary>
        /// Will construct a definitive grammar from an input string, throw exception with compiler error otherwise.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startSymbol"></param>
        public void setDefinitiveGrammar(String input, String startSymbol)
        {
            setDefinitiveGrammar(input);
            setStartSymbol(startSymbol);
        }

        public void setStartSymbol(string name)
        {
            NonterminalHeadD nont = definitiveGrammar.getNonterminal(name);
            if (nont != null)
            {
                definitiveGrammar.setStartSymbol(nont);
            }
            else
                throw new Exception("Could not find the Nonterminal from the name [" + name + "] given.");

            visualisedGrammar = null;
        }

        public String getStartSymbol()
        {
            return definitiveGrammar.StartSymbol.Name;
        }

        public List<String> getNonterminalNames()
        {
            return definitiveGrammar.getNonterminalNames();            
        }
    }
}
