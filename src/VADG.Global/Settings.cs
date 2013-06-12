using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
namespace VADG.Global
{
    public class Settings
    {
        private static Color Level0 = Color.FromRgb(200, 200, 200); // grey
        private static Color Level1 = Color.FromRgb(204, 255, 255); //turqoise
        private static Color Level2 = Color.FromRgb(153, 204, 255); // blue
        private static Color Level3 = Color.FromRgb(153, 255, 153); // green
        private static Color Level4 = Color.FromRgb(255, 204, 51); // yellow
        private static Color Level5 = Color.FromRgb(255, 102, 102); //red


        public static Brush getBrush(int level)
        {
            level = level % 6;
            switch (level) 
            {
                case 0: return new SolidColorBrush(Level0);
                case 1: return new SolidColorBrush(Level1);
                case 2: return new SolidColorBrush(Level2);
                case 3: return new SolidColorBrush(Level3);
                case 4: return new SolidColorBrush(Level4);
                case 5: return new SolidColorBrush(Level5);
               
                default: break;
            }

            return Brushes.White;
        }

        public static String AssignmentOperator = "=";
        public static String ChoiceOperator = "|";
        public static String ConcatOperator = ",";
        public static String EndOfRuleSymbol = ";";
        public static String TerminalEncapsulation = "\'";

        public static bool ConcatIsWhiteSpace()
        {
            return (ConcatOperator.Equals(" ") || ConcatOperator.Equals("\n") || ConcatOperator.Equals("\t"));
        }
    }
}
