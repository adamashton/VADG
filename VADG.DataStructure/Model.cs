//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VADG.DataStructure
//{
//    /// <summary>
//    /// The main interface to the grammar data structures. The Model in the MVC.
//    /// </summary>
//    public class Model
//    {
//        #region Variables
//        private VisualisedGrammar visualisedGrammar;
//        public VisualisedGrammar VisualisedGrammar
//        {
//            get
//            {
//                if (visualisedGrammar == null)
//                    buildVisualisedGrammar();

//                return visualisedGrammar;
//            }
//        }

//        private DefinitiveGrammar definitiveGrammar;
//        public DefinitiveGrammar DefinitiveGrammar
//        {
//            get { return definitiveGrammar; }
//        }

//        private Functions functions;
//        public Functions Functions
//        {
//            get
//            {
//                if (functions == null)
//                    functions = new Functions();
//                return functions;
//            }
//        }

//        #endregion

//        #region Constructors
//        public Model()
//        {
//            Init(null);
//        }

//        public Model(DefinitiveGrammar definitiveGrammarIn)
//        {
//            Init(definitiveGrammarIn);
//        }

//        private void Init(DefinitiveGrammar definitiveGrammarIn)
//        {
//            definitiveGrammar = definitiveGrammarIn;
//        }

//        #endregion

//        private void buildVisualisedGrammar()
//        {
//            if (definitiveGrammar == null)
//                throw new Exception("You must set the definitive grammar first");

//            visualisedGrammar = new VisualisedGrammar(definitiveGrammar);
//        }

//        public void setDefinitiveGrammar(DefinitiveGrammar dg)
//        {
//            definitiveGrammar = dg;
//            visualisedGrammar = null;
//        }

//        /// <summary>
//        /// Will construct a definitive grammar from an input string, throw exception with compiler error otherwise.
//        /// </summary>
//        /// <param name="input"></param>
//        public void setDefinitiveGrammar(String input)
//        {

//        }
        
//    }
//}
