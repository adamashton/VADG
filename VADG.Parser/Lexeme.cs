using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.Parser
{
    public class Lexeme
    {
        private int lineNumber;
        private LexemeType type;
        private String value;

        /// <summary>
        /// Returns the line number to which this lexeme is located.
        /// </summary>
        public int LineNumber
        {
            get { return lineNumber; }
        }

        public LexemeType Type
        {
            get { return type; }
        }

        public String Value
        {
            get { return value; }
        }

        public Lexeme(LexemeType typeIn, int lineNoIn)
        {
            Init(typeIn, null, lineNoIn);
        }

        public Lexeme(LexemeType typeIn, String valueIn, int lineNoIn)
        {
            Init(typeIn, valueIn, lineNoIn);
        }

        private void Init(LexemeType typeIn, String valueIn, int lineNoIn)
        {
            type = typeIn;
            value = valueIn;
            lineNumber = lineNoIn;
        }

        public override string ToString()
        {
            if (value != null)
                return "(" + type.ToString() + ", " + value.ToString() + ")";
            else
                return "(" + type.ToString() + ", )";
        }

        public String ToValueString()
        {
            switch (type)
            {
                case LexemeType.Concatenation:
                    return VADG.Global.Settings.ConcatOperator + " ";
                case LexemeType.Choice:
                    return " " + VADG.Global.Settings.ChoiceOperator + " ";
                case LexemeType.Nonterminal:
                    return value;
                case LexemeType.Terminal:
                    return VADG.Global.Settings.TerminalEncapsulation + value + VADG.Global.Settings.TerminalEncapsulation;
                case LexemeType.Assignment:
                    return VADG.Global.Settings.AssignmentOperator;
                case LexemeType.Undefined:
                    return "???";

                default:
                    return ToString();
            }
        }

    }
}
