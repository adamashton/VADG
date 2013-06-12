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
using VADG.Global;

namespace VADG.Drawer
{
    /// <summary>
    /// Interaction logic for NonterminalCollapsed.xaml
    /// </summary>
    public partial class NonterminalCollapsed : UserControl
    {
        private NonterminalTailV reference;
        /// <summary>
        /// The reference to the nonterminal in visualised grammar
        /// </summary>
        public NonterminalTailV Reference
        {
            get { return reference; }
        }

        public delegate void ControlEventHandler(object sender, GrammarEventArgs e);
        /// <summary>
        /// The main event when something happens to a nonterminal collapsed.
        /// </summary>
        public event ControlEventHandler ControlEvent;

        public NonterminalCollapsed(NonterminalTailV nonterminalTailIn)
        {
            InitializeComponent();

            Init(nonterminalTailIn);
        }

        private void Init(NonterminalTailV nonterminalTailIn)
        {
            reference = nonterminalTailIn;

            labelName.Content = "_" + reference.Name;

            this.Background = Global.Settings.getBrush(reference.Level);

            if (reference.Reference.Reference.IsSelected)
                SetBorderThickness(2.0);

            if ((reference.Reference.Parent.Parent as ChoiceD).Children.Count == 1)
                rightClickMenu.Items.Remove(removeChoice);
           

            // let the controller hear me y'all
            ViewController.ViewListener.Listen(this);
        }

        private void SetBorderThickness(double p)
        {
            this.BorderThickness = new Thickness(p);
        }

        private void expandBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ControlEvent != null)
            {
                GrammarEventArgs eargs = new GrammarEventArgs();
                eargs.Reference = reference;
                //eargs.MyParent = this.Parent as StackPanel;
                eargs.Action = GrammarActions.Expand;
                ControlEvent(this, eargs);
            }
        }

        private void uControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void uControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
                FireEvent(GrammarActions.Click);
            else if (e.ClickCount == 2)
                FireEvent(GrammarActions.DoubleClick);
        }

        private void FireEvent(GrammarActions action)
        {
            if (ControlEvent != null)
            {
                GrammarEventArgs e = new GrammarEventArgs();
                e.Action = action;
                e.Reference = reference;
                e.SymbolType = SymbolType.Nonterminal;
                e.setRule(reference);
                
                ControlEvent(this, e); 
            }
        }

        private void addAfter_Click(object sender, RoutedEventArgs e)
        {
            FireEvent(GrammarActions.AddAfter);
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            FireEvent(GrammarActions.Delete);
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
