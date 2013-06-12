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
using System.Windows.Threading;
using VADG.Global;

namespace VADG.Drawer
{
    /// <summary>
    /// Interaction logic for Terminal.xaml
    /// </summary>
    public partial class Terminal : UserControl
    {
        #region Variables

        private TerminalV terminalV;
        public TerminalV TerminalV
        {
            get { return terminalV; }
        }

        #endregion

        public delegate void ControlEventHandler(object sender, GrammarEventArgs e);
        /// <summary>
        /// The main event when something happens to a terminal
        /// </summary>
        public event ControlEventHandler ControlEvent;

        
        public Terminal(TerminalV terminalVIn)
        {
            InitializeComponent();

            Init(terminalVIn);
        }

        private void Init(TerminalV terminalVIn)
        {
            terminalV = terminalVIn;           

            SetLabelContent(terminalV.Name);

            this.Background = VADG.Global.Settings.getBrush(terminalV.Level);

            if (terminalV.Reference.IsSelected)
            {
                SetBorderThickness(2.0);
            }

            if ((terminalV.Reference.Parent.Parent as ChoiceD).Children.Count == 1)
                rightClickMenu.Items.Remove(removeChoice);
           

            // listen to events from the controller
            ViewController.ViewEvents.TerminalEvent += new ViewEvents.TerminalEventHandler(ViewEvents_TerminalEvent);

            // let the listener controller hear me :)
            ViewController.ViewListener.Listen(this);
        }

        private void SetBorderThickness(double p)
        {
            this.BorderThickness = new Thickness(2.0);
        }

        void ViewEvents_TerminalEvent(object sender, GrammarEventArgs e)
        {
            // if the reference is the same
            if (e.Reference == terminalV)
            {
                switch (e.Action)
                {
                    case GrammarActions.Rename:
                        SetLabelContent(e.NewName);                        
                        break;
                    default: throw new Exception("The action " + e.Action + " is not supported by the Terminal Control");
                }
            }
        }   

        private void SetLabelContent(string name)
        {
            
            label.Content = "_" + VADG.Global.Settings.TerminalEncapsulation + name + VADG.Global.Settings.TerminalEncapsulation;
            terminalControl.Content = label;
        }


        private void FireEvent(GrammarActions action)
        {
            if (ControlEvent != null)
            {
                GrammarEventArgs eArgs = new GrammarEventArgs();
                eArgs.Action = action;
                eArgs.Reference = terminalV;
                eArgs.setRule(terminalV);

                ControlEvent(this, eArgs);
            }
        }

        private void terminalControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FireEvent(GrammarActions.Click);
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
