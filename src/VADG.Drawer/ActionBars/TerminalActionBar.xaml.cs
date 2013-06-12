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
    /// Interaction logic for TerminalActionBar.xaml
    /// </summary>
    public partial class TerminalActionBar : UserControl
    {
        Terminal terminalControl;

        public delegate void ControlEventHandler(object sender, GrammarEventArgs e);
        /// <summary>
        /// The main event when something happens to a terminal
        /// </summary>
        public event ControlEventHandler ControlEvent;

        private void FireEvent(GrammarActions action)
        {
            if (ControlEvent != null)
            {
                GrammarEventArgs eventArgs = new GrammarEventArgs();
                eventArgs.Action = action;
                eventArgs.Reference = terminalControl.TerminalV;
                eventArgs.NewName = nameTextBox.Text;
                eventArgs.setRule(terminalControl.TerminalV);
                eventArgs.SymbolType = SymbolType.Terminal;
                
                ControlEvent(terminalControl, eventArgs);
            }
        }

        public TerminalActionBar(Terminal terminalControlIn)
        {
            InitializeComponent();

            Init(terminalControlIn);
        }

        private void Init(Terminal terminalControlIn)
        {
            terminalControl = terminalControlIn;
            SetName(terminalControl.TerminalV.Name);
        }

        private void SetName(String name)
        {
            nameTextBox.Text = name;
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
