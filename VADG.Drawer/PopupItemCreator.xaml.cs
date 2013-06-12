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

namespace VADG.Drawer
{
    /// <summary>
    /// Interaction logic for PopupItemCreator.xaml
    /// </summary>
    public partial class PopupItemCreator : Window
    {
        List<String> symbolNames;

        /// <summary>
        /// We need the symbol names to calculate the distance algorithm :-)
        /// </summary>
        /// <param name="symbolNames"></param>
        public PopupItemCreator(List<String> symbolNamesIn)
        {
            InitializeComponent();
            symbolNames = symbolNamesIn;
        }
        
        private bool confirmChange = false;
        /// <summary>
        /// Has the user confirmed they want to change the undefined item.
        /// </summary>
        public bool ConfirmChange
        {
            get { return confirmChange; }
        }

        private VADG.Global.SymbolType symboltype;
        public VADG.Global.SymbolType SymbolType
        {
            get { return symboltype; }
        }

        public String ControlName
        {
            get { return inputName.Text; }
            set { inputName.Text = value; }
        }
        
       

        private void terminalButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(ControlName))
            {
                MessageBox.Show("You must choose a valid control name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            symboltype = VADG.Global.SymbolType.Terminal;
            confirmChange = true;
            
            this.Close();
        }

        private void nontButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;

                if (String.IsNullOrEmpty(ControlName))
                {
                    MessageBox.Show("You must choose a valid control name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                symboltype = VADG.Global.SymbolType.Nonterminal;
                confirmChange = true;

                // distance!
                if (ControlName.Length >= 4 && !symbolNames.Contains(ControlName))
                {
                    // new nont with length >= 4
                    String suggestion = VADG.Global.LevenshteinDistanceCalculator.suggest(ControlName, symbolNames);
                    if (suggestion != null)
                    {
                        MessageBoxResult result = MessageBox.Show("Did you mean: '" + suggestion + "'?", "Suggestion", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Cancel)
                            return;

                        if (result == MessageBoxResult.Yes)
                            ControlName = suggestion;
                    }
                }

                this.Close();
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
