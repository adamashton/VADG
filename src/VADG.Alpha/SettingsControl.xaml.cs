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

namespace VADG.Alpha
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : Window
    {
        public SettingsControl()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            assignmentOperator.Text = VADG.Global.Settings.AssignmentOperator;
            concatenationOperator.Text = VADG.Global.Settings.ConcatOperator;
            choiceOperator.Text = VADG.Global.Settings.ChoiceOperator;
            endOfRuleSymbol.Text = VADG.Global.Settings.EndOfRuleSymbol;
            terminalEncapsulation.Text = VADG.Global.Settings.TerminalEncapsulation;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            VADG.Global.Settings.AssignmentOperator = assignmentOperator.Text;
            VADG.Global.Settings.ConcatOperator = concatenationOperator.Text;
            VADG.Global.Settings.ChoiceOperator = choiceOperator.Text;
            VADG.Global.Settings.EndOfRuleSymbol = endOfRuleSymbol.Text;
            VADG.Global.Settings.TerminalEncapsulation = terminalEncapsulation.Text;

            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
