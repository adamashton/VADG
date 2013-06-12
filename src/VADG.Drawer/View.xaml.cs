using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VADG.DataStructure;
using VADG.Model;

namespace VADG.Drawer
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        private GrammarModel model;
        private Functions functions;
        /// <summary>
        /// exposes methods to manipulate the view
        /// </summary>
        public Functions Functions
        {
            get
            {
                if (functions == null)
                    functions = new Functions(model.VisualisedGrammar.StartSymbol);

                return functions;
            }
        }

        
        
        public View(GrammarModel modelIn)
        {
            InitializeComponent();
            
            Init(modelIn);
        }

        private void Init(GrammarModel modelIn)
        {
            model = modelIn;

            ViewController.ReInit(); 

            // draw controls
            GrammarNodeVDrawVisitor visitor = new GrammarNodeVDrawVisitor();
            model.VisualisedGrammar.accept(visitor);            
            this.Content = visitor.DrawnItem;

            // update global information
            VADG.Global.Information.Nonterminals = model.getNonterminalNames();
            
        }
    }
}
