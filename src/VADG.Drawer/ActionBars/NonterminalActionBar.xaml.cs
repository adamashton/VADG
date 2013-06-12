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
    /// Interaction logic for NonterminalActionBar.xaml
    /// </summary>
    public partial class NonterminalActionBar : UserControl
    {
        NonterminalCollapsed nontCollapsed;
        NonterminalExpanded nontExpanded;

        /// <summary>
        /// Calculates what refrence this relates to
        /// </summary>
        /// <returns></returns>
        private SymbolV getReference()
        {
            if (nontCollapsed != null)
            {
                return nontCollapsed.Reference;
            }
            else if (nontExpanded != null)
            {
                if (nontExpanded.ReferenceHead != null)
                    return nontExpanded.ReferenceHead;
                else if (nontExpanded.ReferenceTail != null)
                    return nontExpanded.ReferenceTail;
            }

            throw new Exception("Error in the nont action bar, it does not ref anything :'(");
        }

        public delegate void ControlEventHandler(object sender, GrammarEventArgs e);
        /// <summary>
        /// The main event when something happens to a terminal
        /// </summary>
        public event ControlEventHandler ControlEvent;

        private void FireEvent(GrammarActions action)
        {
            if (ControlEvent != null)
            {
                GrammarEventArgs eArgs = new GrammarEventArgs();
                eArgs.Action = action;
                eArgs.Reference = getReference();
                eArgs.NewName = nameTextBox.Text;
                eArgs.SymbolType = SymbolType.Nonterminal;

                if (nontExpanded != null)
                {
                    if (nontExpanded.ReferenceTail != null)
                        eArgs.setRule(nontExpanded.ReferenceTail);
                    else
                        eArgs.setRule(nontExpanded.ReferenceHead);

                    ControlEvent(nontExpanded, eArgs);
                }
                else
                {
                    eArgs.setRule(nontCollapsed.Reference);
                    ControlEvent(nontCollapsed, eArgs);
                }
            }            
        }
        
        public NonterminalActionBar(NonterminalCollapsed nontCollapsedIn)
        {
            InitializeComponent();

            nontCollapsed = nontCollapsedIn;

            // nontCollapsed -> nontV -> nontTailD -> nontHeadD
            Init(nontCollapsedIn.Reference.Reference.Reference);
        }

        

        

        public NonterminalActionBar(NonterminalExpanded nontExpandedIn)
        {
            InitializeComponent();

            nontExpanded = nontExpandedIn;

            Init(nontExpandedIn.NonterminalHeadD);
        }

        private void Init(NonterminalHeadD nonterminalHeadD)
        {
            this.nameTextBox.Text = nonterminalHeadD.Name;
        }

        private void renameAll_Click(object sender, RoutedEventArgs e)
        {
            FireEvent(GrammarActions.Rename);
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
    }
}
