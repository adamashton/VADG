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

namespace VADG.Drawer
{
    /// <summary>
    /// Interaction logic for Concatenation.xaml
    /// </summary>
    public partial class Concatenation : UserControl, OperationVisualised
    {
        public UIElementCollection Children
        {
            get { return stackPanel.Children; }
        }
        
        public Concatenation(List<UserControl> childrenIn, bool directionIn)
        {
            InitializeComponent();

            Init(childrenIn, directionIn);
            
        }

        private void Init(List<UserControl> childrenIn, bool directionIn)
        {
            FillStackPanel(childrenIn);

            if (directionIn)
                stackPanel.Orientation = Orientation.Vertical;
        }

        private void FillStackPanel(List<UserControl> childrenIn)
        {
            foreach (UserControl child in childrenIn)
            {
                stackPanel.Children.Add(child);
            }

            
        }

    }
}
