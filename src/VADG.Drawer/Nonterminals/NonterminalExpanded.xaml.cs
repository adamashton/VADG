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
using System.Windows.Interop;
using VADG.Global;

namespace VADG.Drawer
{
    /// <summary>
    /// Interaction logic for NonterminalExpanded.xaml
    /// </summary>
    public partial class NonterminalExpanded : UserControl
    {
        // the visualisation references either ruleTail or ruleHead depending on the start symbol or not
        NonterminalTailV referenceTail;
        /// <summary>
        /// The reference to this nonterminals NonterminalTailV, is != null iff this != StartSymbol.
        /// </summary>
        public NonterminalTailV ReferenceTail
        {
            get { return referenceTail; }
        }
        
        NonterminalHeadV referenceHead;
        public NonterminalHeadV ReferenceHead
        {
            get { return referenceHead; }
        }

        public NonterminalHeadD NonterminalHeadD
        {
            get
            {
                if (isStartSymbol)
                    return referenceHead.Reference;
                else
                    return referenceTail.Reference.Reference;
            }
        }

        bool isStartSymbol;

        UserControl visualisedSymbols;

        public delegate void ControlEventHandler(object sender, GrammarEventArgs e);
        /// <summary>
        /// The main event when something happens to a nonterminal expanded.
        /// </summary>
        public event ControlEventHandler ControlEvent;

        public NonterminalExpanded(NonterminalTailV ruleIn, UserControl visualisedSymbolsIn)
        {
            InitializeComponent();

            Init(ruleIn, visualisedSymbolsIn, false);
        }

        public NonterminalExpanded(NonterminalHeadV ruleIn, UserControl visualisedSymbolsIn)
        {
            InitializeComponent();

            Init(ruleIn, visualisedSymbolsIn, true);
        }

        private void Init(SymbolV ruleIn, UserControl visualisedSymbolsIn, bool isStartSymbolIn)
        {
            visualisedSymbols = visualisedSymbolsIn;
            isStartSymbol = isStartSymbolIn;

            if (isStartSymbol)
            {
                referenceHead = (NonterminalHeadV)ruleIn;
                referenceTail = null;
                
                setName(referenceHead.Name);

                this.Background = Global.Settings.getBrush(0);

                collapseBtn.Visibility = Visibility.Collapsed;

                if (referenceHead.Reference.IsSelected)
                    SetBorderThickness(2.0);

                rightClickMenu.Visibility = Visibility.Collapsed;
            }
            else
            {
                referenceHead = null;
                referenceTail = (NonterminalTailV)ruleIn;

                if ((referenceTail.Reference.Parent.Parent as OperationD).Children.Count == 1)
                    rightClickMenu.Items.Remove(removeChoice);
                
                setName(referenceTail.Name);

                this.Background = Global.Settings.getBrush(referenceTail.Level);

                if (referenceTail.Reference.Reference.IsSelected)
                    SetBorderThickness(2.0);
            }

            // change direction of nont chilren if necessary
            if (ruleIn.Direction)
            {
                // draw top to bottom
                stackPanelOuter.Orientation = Orientation.Horizontal;
                stackPanelHeading.Orientation = Orientation.Vertical;
                stackPanelInner.Orientation = Orientation.Vertical;
                stackPanelName.Orientation = Orientation.Horizontal;
                arrow_down.Visibility = Visibility.Visible;
                arrow_right.Visibility = Visibility.Collapsed;
            }
            
            // add child to GUI
            stackPanelInner.Children.Remove(holder);
            stackPanelInner.Children.Add(visualisedSymbols);

            // let the controller hear me y'all
            ViewController.ViewListener.Listen(this);
        }

        private void SetBorderThickness(double p)
        {
            this.BorderThickness = new Thickness(p);
        }           

        public void setName(String nameIn)
        {
            labelName.Content = new Bold(new Run(nameIn)); 
        }

        // Broadcast when I am collapsed
        private void collapseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isStartSymbol)
            {
                FireEvent(GrammarActions.Collapse);
            }
            else
                MessageBox.Show("You can't collapse the start symbol.", "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        private void flipBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isStartSymbol)
            {
                FireEvent(GrammarActions.Flip);
            }
        }

        private void FireEvent(GrammarActions action)
        {
            GrammarEventArgs e = new GrammarEventArgs();
            e.Action = action;
            if (isStartSymbol)
            {
                e.Reference = referenceHead;
                e.setRule(referenceHead);
            }
            else
            {
                e.Reference = referenceTail;
                e.setRule(referenceTail);
            }
            
            
            if (ControlEvent != null)
                ControlEvent(this, e);
        }

        private void stackPanelName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
                FireEvent(GrammarActions.Click);
            else if (e.ClickCount == 2)
                FireEvent(GrammarActions.DoubleClick);
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            FireEvent(GrammarActions.Delete);
        }

        private void addAfter_Click(object sender, RoutedEventArgs e)
        {
            FireEvent(GrammarActions.AddAfter);
        }

        private void addBefore_Click(object sender, RoutedEventArgs e)
        {
            FireEvent(GrammarActions.AddBefore);
        }

        private void addChoice_Click(object sender, RoutedEventArgs e)
        {
            FireEvent(GrammarActions.AddChoice);
        }

        private void removeChoice_Click(object sender, RoutedEventArgs e)
        {
            FireEvent(GrammarActions.RemoveChoice);
        }
    }
}
