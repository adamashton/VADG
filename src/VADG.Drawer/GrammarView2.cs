using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VADG.DataStructure;
using VADG.Model;

namespace VADG.Drawer
{
    /// <summary>
    /// The main interface to the view, in the MVC pattern.
    /// </summary>
    public class GrammarView2 : UserControl
    {
        private GrammarModel model;

        public GrammarView2(GrammarModel modelIn)
        {
            Init(modelIn);
        }

        private void Init(GrammarModel modelIn)
        {
            model = modelIn;

            GrammarNodeVDrawVisitor visitor = new GrammarNodeVDrawVisitor();
            model.VisualisedGrammar.StartSymbol.accept(visitor);
        }
    }
}
