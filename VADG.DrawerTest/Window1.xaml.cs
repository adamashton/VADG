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
using VADG.Drawer;
using VADG.Controller;
using VADG.Model;
    
namespace VADG.DrawerTest
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

            Initialise();

            SetView();
        }

        private void Initialise()
        {
            model = new GrammarModel();
            model.setDefinitiveGrammar(TestGrammars.Basic);
        }

        private void SetView()
        {
            view = new View(model);
            controller = new GrammarController(model, view);
            
            // set text
            grammarText.Text = model.DefinitiveGrammar.PrintGrammar();

            // add component
            grid1.Children.Clear();
            grid1.Children.Add(view);
        }

      
        private void grammar_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Name == "grammar0")
            {
                model.setDefinitiveGrammar(TestGrammars.Basic);
                SetView();
            }
            else if ((sender as Button).Name == "grammar1")
            {
                model.setDefinitiveGrammar(TestGrammars.LongNames);
                SetView();
            }
            else if ((sender as Button).Name == "grammar2")
            {
                model.setDefinitiveGrammar(TestGrammars.Recursive);
                SetView();
            }
            else if ((sender as Button).Name == "grammar3")
            {
                model.setDefinitiveGrammar(TestGrammars.SimpleChoice);
                SetView();
            }
            else if ((sender as Button).Name == "grammar4")
            {
                model.setDefinitiveGrammar(TestGrammars.MathOperation);
                SetView();
            }
            else
                throw new Exception();
        }

    }
}
