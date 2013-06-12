using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.DataStructure;
using VADG.Global;

namespace VADG.Model
{
    /// <summary>
    /// A static class that generates some default grammars used for testing
    /// </summary>
    public static class TestGrammars
    {
        public static DefinitiveGrammar Basic
        {
            get { return basic(); }
        }
        public static DefinitiveGrammar LongNames
        {
            get { return intermediate(); }
        }
        public static DefinitiveGrammar Recursive
        {
            get { return recurisve(); }
        }
        public static DefinitiveGrammar SimpleChoice
        {
            get { return simpleChoice(); }
        }
        public static DefinitiveGrammar MathOperation
        {
            get { return mathOperation(); }
        }


        private static DefinitiveGrammar basic()
        {
            String grammar = "S" + Settings.AssignmentOperator + terminal("A") + Settings.EndOfRuleSymbol;
            return parseText(grammar);
        }

        private static DefinitiveGrammar intermediate()
        {
            String grammar = "S" + Settings.AssignmentOperator + "A" + Settings.ConcatOperator + "B" + Settings.ConcatOperator + "C" + Settings.EndOfRuleSymbol
                             + "A" + Settings.AssignmentOperator + terminal("A") + Settings.EndOfRuleSymbol
                             + "B" + Settings.AssignmentOperator + terminal("B") + Settings.EndOfRuleSymbol
                             + "C" + Settings.AssignmentOperator + terminal("C") + Settings.EndOfRuleSymbol;

            return parseText(grammar);
        }

        private static DefinitiveGrammar recurisve()
        {
            String grammar = "S" + Settings.AssignmentOperator + "A" + Settings.ConcatOperator + "B" + Settings.ConcatOperator + "C" + Settings.EndOfRuleSymbol
                             + "A" + Settings.AssignmentOperator + terminal("A") + Settings.ConcatOperator + "S" + Settings.EndOfRuleSymbol
                             + "B" + Settings.AssignmentOperator + terminal("B") + Settings.EndOfRuleSymbol
                             + "C" + Settings.AssignmentOperator + "S" + Settings.EndOfRuleSymbol;
            return parseText(grammar);
        }

        private static DefinitiveGrammar simpleChoice()
        {
            String grammar = "MyChoice" + Settings.AssignmentOperator + terminal("option1") + Settings.ChoiceOperator + terminal("option2") + Settings.ChoiceOperator + "Option3" + Settings.EndOfRuleSymbol
                            + "Option3" + Settings.AssignmentOperator + terminal("Option3") + Settings.EndOfRuleSymbol;

            return parseText(grammar);
        }

        private static DefinitiveGrammar mathOperation()
        {
            String grammar = "MathOperation = Number, Operator, Number | Number, Operator, MathOperation; Number = '[0-9]' | '[1-9][0-9]+'; Operator = '+' | '-' | '/' | '*';";
            
            return parseText(grammar);
        }

        private static DefinitiveGrammar parseText(String grammar)
        {
            VADG.Parser.BNFParser bnfParser = new VADG.Parser.BNFParser();
            bool noError = bnfParser.Parse(grammar);
            if (noError)
                return new DefinitiveGrammar(bnfParser.Rules, bnfParser.Rules[0]);
            else
                throw new Exception(bnfParser.ErrorText);
        }

        private static String terminal(String name)
        {
            return Settings.TerminalEncapsulation + name + Settings.TerminalEncapsulation;
        }
    }
}
