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
using VADG.Model;
using VADG.Drawer;
using VADG.Controller;
using VADG.DataStructure;
using VADG.Model.Analysis;

namespace VADG.Alpha
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private GrammarModel model;
        private View view;
        private GrammarController controller;

        public Window1()
        {
            InitializeComponent();

            model = new GrammarModel();
        }

        private void SetView()
        {
            // assumes definitive grammar has been set, now setting up view of program
            
            // set start symbol comboBox
            setStartSymbolChoices();

            model.setStartSymbol(startSymbolChoice.SelectedItem as String);

            ReText();
            Redraw();      
        }

        void Refresh(object sender, EventArgs e)
        {
            ReText();
            Redraw();
        }

        private void Redraw()
        {
            try
            {
                // draw and set up controller
                view = new View(model);
                controller = new GrammarController(model, view);
                controller.RedrawEvent += new GrammarController.ReDrawEventHandler(Refresh);
                controller.ControlSelected += new GrammarController.ControlSelectedHandler(setActionBar);
                controller.ErrorSerious += new GrammarController.ErrorEventHandler(controller_ErrorEventSerious);
                controller.OfferDeleteRule += new GrammarController.OfferDeleteRuleHandler(controller_OfferDeleteRule);
                controller.Error += new GrammarController.ErrorEventHandler(controller_Error);

                grammarContent.Content = view;

                setStartSymbolChoices();
            }
            catch (Exception e)
            {
                controller_ErrorEventSerious(e);
            }
        }

        

        void controller_OfferDeleteRule(NonterminalHeadD rule)
        {
            string message = "The rule '" + rule.Name + "' is not used within your grammar any more."
                    + "\nDo you want to remove the rule now?";

            MessageBoxResult result = MessageBox.Show(message, "Rule Deletion Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes)
            {
                model.Functions.DeleteRule(rule);
            }
        }

        void controller_ErrorEventSerious(Exception exception)
        {
            controller_Error(exception);

            // set up default
            model = new GrammarModel(TestGrammars.Basic);
            SetView();
        }

        void controller_Error(Exception exception)
        {
            MessageBoxResult result = MessageBox.Show(exception.Message + "\n\nDo you want to see more details?", "Error Occured", MessageBoxButton.YesNo, MessageBoxImage.Error);

            if (result == MessageBoxResult.Yes)
            {
                String message = "";
                try
                {
                    message = "Error Message: " + exception.Message
                                + "\nSource: " + exception.Source
                                + "\nTarget Site : " + exception.TargetSite.ToString()
                                + "\nStack Tracke: " + exception.StackTrace;
                }
                catch
                {
                    // show simple error
                    message = exception.ToString();
                }
                
                
                MessageBox.Show(message, "Error Details", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        void setActionBar(object sender, GrammarEventArgs e)
        {
            if (sender is Terminal)
            {
                TerminalActionBar tab = new TerminalActionBar(sender as Terminal);
                actionBarHolder.Content = tab;
                controller.setActionBar(tab);
            }

            if (sender is NonterminalCollapsed)
            {
                NonterminalActionBar tab = new NonterminalActionBar(sender as NonterminalCollapsed);
                actionBarHolder.Content = tab;
                controller.setActionBar(tab);
            }

            if (sender is NonterminalExpanded)
            {
                NonterminalActionBar tab = new NonterminalActionBar(sender as NonterminalExpanded);
                actionBarHolder.Content = tab;
                controller.setActionBar(tab);
            }
        }

        

        private void ReText()
        {
            // set text
            //grammarText.Text = model.DefinitiveGrammar.PrintGrammar();

            grammarText.Document = model.DefinitiveGrammar.PrintGrammarRich();
        }


        private void setStartSymbolChoices()
        {
            startSymbolChoice.Items.Clear();

            foreach (String nontName in model.getNonterminalNames())
                startSymbolChoice.Items.Add(nontName);

            startSymbolChoice.SelectedItem = model.getStartSymbol();
        }

        private void BasicGrammarButton_Click(object sender, RoutedEventArgs e)
        {
            model.setDefinitiveGrammar(TestGrammars.Basic);
            SetView();
        }

        private void LongGrammarButton_Click(object sender, RoutedEventArgs e)
        {
            model.setDefinitiveGrammar(TestGrammars.LongNames);
            SetView();
        }

        private void RecursiveGrammarButton_Click(object sender, RoutedEventArgs e)
        {
            model.setDefinitiveGrammar(TestGrammars.Recursive);
            SetView();
        }

        private void ChoiceGrammarButton_Click(object sender, RoutedEventArgs e)
        {
            model.setDefinitiveGrammar(TestGrammars.SimpleChoice);
            SetView();
        }

        private void MathOperationGrammarButton_Click(object sender, RoutedEventArgs e)
        {
            model.setDefinitiveGrammar(TestGrammars.MathOperation);
            SetView();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsControl sc = new SettingsControl();
            sc.ShowDialog();
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                try
                {
                    String file = System.IO.File.ReadAllText(ofd.FileName);
                    grammarText.Document = new FlowDocument(new Paragraph(new Run(file)));
                    model.setDefinitiveGrammar(file);
                    SetView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            Nullable<bool> result = sfd.ShowDialog();
            if (result == true)
            {
                try
                {
                    TextRange tr = new TextRange(grammarText.Document.ContentStart, grammarText.Document.ContentEnd);
                    System.IO.File.WriteAllText(sfd.FileName, tr.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to Exit?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                this.Close();
        }

        private void CompileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;
                TextRange tr = new TextRange(grammarText.Document.ContentStart, grammarText.Document.ContentEnd);
                model.setDefinitiveGrammar(tr.Text);
                SetView();
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show(ex.Message, "Compiler Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }    
        }       

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                model.setStartSymbol(startSymbolChoice.SelectedItem as String);
                Redraw();
            }
            catch (Exception ex)
            {
                controller_ErrorEventSerious(ex);
            }
        }

        private void newGrammar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                model.setDefinitiveGrammar("S = ???;", "S");
                SetView();
            }
            catch (Exception ex)
            {
                controller_ErrorEventSerious(ex);
            }
        }

        private void analyseFactorise_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    String message;
            //    List<SubRule> subRules = VADG.Model.Analysis.FactorisationAnalyser.Analyse(model.DefinitiveGrammar);
            //    if (subRules != null)
            //    {
            //        message = "There is a possible factorisation that can be performed"
            //                        + "\nThe rules: ";
            //        for (int i = 0; i < subRules.Count; i++)
            //        {
            //            message += subRules[i].NonterminalHeadDs.Name;

            //            if (i != subRules.Count - 1)
            //                message += ", ";
            //        }

            //        message += "\nHave the same rule body: " + subRules[0].ToString();
            //    }
            //    else
            //        message = "There were no factorisations found.";

            //    MessageBox.Show(message, "Factorisation Analysis Results", MessageBoxButton.OK, MessageBoxImage.Hand);

            //}
            //catch (Exception ex)
            //{
            //    controller_Error(ex);
            //}
        }
        

        private void analyse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;
                GrammarAnalysis ga = new GrammarAnalysis(model);
                this.Cursor = Cursors.Arrow;
                ga.ShowDialog();

                if (ga.applyNewGrammar)
                {
                    model.setDefinitiveGrammar(ga.NewGrammar.DefinitiveGrammar);
                    SetView();
                }
            }
            catch (Exception ex)
            {
                controller_Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bottomBar.Visibility = Visibility.Collapsed;
        }

        private void showBarButton_Click(object sender, RoutedEventArgs e)
        {
            bottomBar.Visibility = Visibility.Visible;
        }
    }
}
