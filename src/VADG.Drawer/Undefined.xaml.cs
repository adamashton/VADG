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
    /// Interaction logic for Undefined.xaml
    /// </summary>
    public partial class Undefined : UserControl
    {
        private UndefinedV reference;
        public UndefinedV Reference
        {
            get { return reference; }
        }

        private string newName = "";
        private SymbolType symbolType = SymbolType.Undefined;

        public delegate void ControlEventHandler(object sender, GrammarEventArgs e);
        /// <summary>
        /// The main event when something happens to an undefined
        /// </summary>
        public event ControlEventHandler ControlEvent;

        public Undefined(UndefinedV undefinedV)
        {
            InitializeComponent();

            reference = undefinedV;

            // listen to events from the controller
            //ViewController.ViewEvents ...

            // let the listener controller hear me :)
            ViewController.ViewListener.Listen(this);

            System.Windows.Controls.ToolTip toolTip1 = new System.Windows.Controls.ToolTip();
            toolTip1.Content = "Click to Change";
            this.ToolTip = toolTip1;


            if ((reference.Reference.Parent as OperationD).Children.Count == 1)
            {
                // the undefined is the only item in this structure
                // do not offer chance to be removed
                rightClickMenu.Items.Remove(delete);
            }

            if ((reference.Reference.Parent.Parent as ChoiceD).Children.Count == 1)
            {
                // do not allow remove choice
                rightClickMenu.Items.Remove(removeChoice);
            }
        }

        private void FireEvent(GrammarActions action)
        {
            if (ControlEvent != null)
            {
                GrammarEventArgs e = new GrammarEventArgs();
                e.Action = action;
                e.NewName = newName;
                e.SymbolType = symbolType;
                e.Reference = reference;
                e.setRule(reference);
                ControlEvent(this, e);
            }
        }

        private void label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            PopupItemCreator ic = new PopupItemCreator(Information.Nonterminals);
            ic.ShowDialog();
            if (ic.ConfirmChange)
            {
                newName = ic.ControlName;
                symbolType = ic.SymbolType;

                FireEvent(GrammarActions.Replace);
            }
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
