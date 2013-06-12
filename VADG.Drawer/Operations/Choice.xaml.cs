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

namespace VADG.Drawer
{
    /// <summary>
    /// Interaction logic for Choice.xaml
    /// </summary>
    public partial class Choice : UserControl, OperationVisualised
    {
        public UIElementCollection Children
        {
            get { return stackPanel.Children; }
        }

        public Choice(List<UserControl> childrenIn, bool directionIn)
        {
            InitializeComponent();

            FillStackPanel(childrenIn, directionIn);
        }

        private void FillStackPanel(List<UserControl> childrenIn, bool directionIn)
        {
            int count = 0;
            foreach (UserControl child in childrenIn)
            {
                stackPanel.Children.Add(child);

                if (count != childrenIn.Count - 1)
                    stackPanel.Children.Add(new Or());

                count++;
            }

            if (directionIn)
                stackPanel.Orientation = Orientation.Horizontal;
        }
    }
}
