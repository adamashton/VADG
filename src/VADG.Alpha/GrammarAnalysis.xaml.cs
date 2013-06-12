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
using System.Windows.Shapes;
using VADG.Model;
using VADG.Model.Analysis;
using VADG.DataStructure;

namespace VADG.Alpha
{
    /// <summary>
    /// Interaction logic for GrammarAnalysis.xaml
    /// </summary>
    public partial class GrammarAnalysis : Window
    {
        GrammarModel model;

        private GrammarModel newGrammar;

        public GrammarModel NewGrammar
        {
            get { return newGrammar; }
            set { newGrammar = value; }
        }

        public bool applyNewGrammar = false;
        
        public GrammarAnalysis(GrammarModel modelIn)
        {
            InitializeComponent();

            model = modelIn;

            currentGrammar.Document = model.DefinitiveGrammar.PrintGrammarRich();

            FillAnalysisOptions();
        }

        private void FillAnalysisOptions()
        {
            List<Factorisation> factorisations = FactorisationAnalyser.Analyse(model.DefinitiveGrammar);

            if (factorisations == null)
            {
                MessageBox.Show("There are no factorisations that can be performed.","No Factorisations", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            analysisOptions.ItemsSource = factorisations;
        }

        private void previewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;

                Factorisation factorisation = analysisOptions.SelectedItem as Factorisation;

                if (factorisation == null)
                    throw new Exception("You did not select a factorisation option");

                String newRule = "NewRule_" + new Random().Next(0, 9999).ToString();

                String currentGrammarStr = model.DefinitiveGrammar.PrintGrammar();
                String newGrammarStr = factorisation.applyRule(currentGrammarStr, newRule);

                GrammarModel newModel = new GrammarModel(newGrammarStr);
                newGrammar = new GrammarModel(newModel.DefinitiveGrammar);

                

                // highlight rules that are being factorised
                // first unselect any rules selected
                GrammarNodeDUnselectVisitor visitor = new GrammarNodeDUnselectVisitor();
                model.DefinitiveGrammar.StartSymbol.accept(visitor);
                newModel.DefinitiveGrammar.StartSymbol.accept(visitor);

                foreach (NonterminalHeadD nont in factorisation.NontHeads)
                {
                    model.DefinitiveGrammar.getNonterminal(nont.Name).IsSelected = true;
                    newModel.DefinitiveGrammar.getNonterminal(nont.Name).IsSelected = true;
                }

                newModel.DefinitiveGrammar.getNonterminal(newRule).IsSelected = true;

                // print grammars
                currentGrammar.Document = model.DefinitiveGrammar.PrintGrammarRich();
                previewGrammar.Document = newModel.DefinitiveGrammar.PrintGrammarRich();

                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            

        }

        private void apply_Click(object sender, RoutedEventArgs e)
        {
            applyNewGrammar = true;
            this.Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            applyNewGrammar = false;
            this.Close();
        }


    }
}
