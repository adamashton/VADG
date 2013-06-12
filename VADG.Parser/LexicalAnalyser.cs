using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VADG.DataStructure;

namespace VADG.Parser
{
    internal class LexicalAnalyser
    {
        // lexical analyser defaults
        public String AssignmentOperator = VADG.Global.Settings.AssignmentOperator;
        public String Choice = VADG.Global.Settings.ChoiceOperator;
        public String Concat = VADG.Global.Settings.ConcatOperator;
        public String EndOfRule = VADG.Global.Settings.EndOfRuleSymbol;
        public String terminalEncapsulation = VADG.Global.Settings.TerminalEncapsulation;

        Regex allowedChars = new Regex(@"[A-Za-z0-9!£%&_*/\+\-\#\@\~\.\[\]]");

        List<Lexeme> lexemes = null;
        public List<Lexeme> Lexemes
        {
            get
            {
                if (lexemes == null)
                    GetLexemes();
                return lexemes;
            }
        }

        // the source file
        private String source;

        // current character location
        private int current;

        // current character
        private String character;

        // next character, if null reached EOF
        private String nextCharacter;

        private String errorMsg = "No Errors.";
        public String ErrorMsg
        {
            get { return errorMsg; }
        }

        int lineCount = 1;

        public LexicalAnalyser(String sourceIn)
        {
            source = sourceIn;
            current = 0;
            character = null;
            nextCharacter = source[current].ToString();
        }

        private void GetLexemes()
        {
            if (lexemes == null)
            {
                lexemes = new List<Lexeme>();
                Lexeme lexeme;
                GetNextChar(); // move forward into position :)
                do
                {
                    lexeme = GetNext();
                    lexemes.Add(lexeme);
                }
                while (lexeme.Type != LexemeType.EOF);


                
            }
        }

        /// <summary>
        /// returns true iff a successful parse :-)
        /// </summary>
        /// <returns></returns>
        public bool Analyse()
        {
            try
            {
                GetLexemes();
                if (lexemes.Count == 0)
                    throw new Exception("No lexemes were found in the file -- empty file/does not exist?");

                errorMsg = "No Errors.";
                return true;
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return false;
            }

        }

        private void GetNextChar()
        {
            character = nextCharacter;
            nextCharacter = null;

            current++;

            if (current < source.Length)
                nextCharacter = source[current].ToString();

            if (character == "\n")
                lineCount++;
        }

        private Lexeme GetNext()
        {
            try
            {
                // check for EOF
                if (character == null)
                    return new Lexeme(LexemeType.EOF, lineCount);


                // ignore whitespace, only move forward if the character is not of type:
                if (character != AssignmentOperator
                    && character != Choice
                    && character != Concat
                    && character != EndOfRule
                    && character != terminalEncapsulation
                    && character != "?"
                    && !allowedChars.IsMatch(character))

                {
                    do
                    {
                        GetNextChar();
                    }
                    while (character == " " || character == "\n" || character == "\t" || character == "\r");
                }

                

                
                // try to parse the assignment operator
                char[] acharacters = AssignmentOperator.ToCharArray();
                if (character == acharacters[0].ToString())
                {
                    // try to parse the assignment operator
                    for (int i = 0; i < acharacters.Length; i++)
                    {
                        if (character != acharacters[i].ToString())
                        {
                            // error when trying to do assignment
                            String message = "Unexpected character [" + character + "], expected [";

                            String completion = "";
                            for (; i < acharacters.Length; i++)
                                completion += acharacters[i];

                            message += completion + "] to complete the assignment operator.";

                            throw new Exception(message);
                        }

                        GetNextChar();
                    }

                    // success :)
                    return new Lexeme(LexemeType.Assignment, lineCount);
                }

                if (character == Concat)
                {
                    GetNextChar();

                    if (VADG.Global.Settings.ConcatIsWhiteSpace())
                        // we ignore concatenations, the syntax analyser will assume no symbol means a concatenation! ooh scary :x
                        return GetNext();                    
                    
                    return new Lexeme(LexemeType.Concatenation, lineCount);                        
                }

                if (character == Choice)
                {
                    GetNextChar();
                    return new Lexeme(LexemeType.Choice, lineCount);
                }

                if (character == EndOfRule)
                {
                    GetNextChar();
                    return new Lexeme(LexemeType.EndOfRule, lineCount);
                }

                // try to parse a terminal
                if (character == terminalEncapsulation)
                {
                    try
                    {
                        GetNextChar();

                        if (character == null)
                            throw new Exception("Unexpected End of File; expected a terminal name followed by a \" before EOF");

                        String terminalValue = "";

                        while (character != null && character != terminalEncapsulation) // allowedChars.IsMatch(character)
                        {
                            terminalValue += character;
                            GetNextChar();
                        }

                        if (character == null)
                            throw new Exception("Unexpected End of File; expected a \" before EOF");

                        if (character == terminalEncapsulation)
                        {
                            // success      
                            GetNextChar();
                            return new Lexeme(LexemeType.Terminal, terminalValue, lineCount);
                        }

                        throw new Exception("Unexpected char [" + character + "] expected [" + terminalEncapsulation + "]");
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error occured when matching a Terminal.\nMessage: " + e.Message);
                    }
                }

                // try to parse a nonterminal
                if (allowedChars.IsMatch(character))
                {
                    try
                    {
                        String nontValue = character;

                        while (nextCharacter != null && allowedChars.IsMatch(nextCharacter))
                        {
                            GetNextChar();

                            nontValue += character;
                        }

                        GetNextChar();
                        return new Lexeme(LexemeType.Nonterminal, nontValue, lineCount);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error occured when matching a Nonterminal.\nMessage: " + e.Message);
                    }
                }


                // try to parse a line comment //
                if (character == "/" && nextCharacter == "/")
                {
                    String commentValue = "";

                    // move forward to start of line comment
                    GetNextChar();
                    GetNextChar();

                    while (character != "\n" && character != "\r")
                    {
                        commentValue += character;
                        GetNextChar();

                        if (nextCharacter == null)
                        {
                            // reached EOF and end of line comment
                            commentValue += character;
                            break;
                        }
                    }

                    return new Lexeme(LexemeType.LineComment, commentValue, lineCount);
                }

                // try to parse multi line comment 
                if (character == "/" && nextCharacter == "*")
                {
                    String commentValue = "";

                    // move to start of comment
                    GetNextChar();
                    GetNextChar();

                    while (character != "*" || nextCharacter != "/")
                    {
                        commentValue += character;
                        GetNextChar();

                        if (nextCharacter == null)
                            throw new Exception("Expected */ before end of file");
                    }

                    GetNextChar(); // move over the /
                    return new Lexeme(LexemeType.Commment, commentValue, lineCount);

                }

                if (character == "?")
                {
                    // possible undefined item
                    GetNextChar();
                    if (character == "?")
                    {
                        GetNextChar();
                        if (character == "?")
                        {
                            GetNextChar();
                            return new Lexeme(LexemeType.Undefined, lineCount);
                        }
                    }
                    
                    throw new Exception("Unexpected character '" + character + "' found in file, expected '???'");
                }


                throw new Exception("Unexpected character '" + character + "' found in file.");
            }
            catch (Exception e)
            {
                String errorMessage = "Error occured on line " + lineCount
                                      + "\nLine: " + GetLine(lineCount)  
                                      + "\nDetails: " + e.Message;

                throw new Exception(errorMessage);
            }
        }

        /// <summary>
        /// Returns line from grammar, 1-n.
        /// </summary>
        /// <param name="lineNo"></param>
        /// <returns></returns>
        public string GetLine(int lineNo)
        {
            if (lineNo <= 0)
                throw new Exception("Line number must be in the range 1-n.");

            int count = 1;
            String line = "";

            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == '\n' || i == source.Length - 1)
                {
                    if (count == lineNo)
                    {
                        line += source[i];
                        return line;
                    }
                    else
                    {
                        count++;
                        i++; // \n                        
                        line = "";
                    }
                }

                line += source[i];
            }

            throw new Exception("The line number " + lineNo + " was not found.");
        }


        /// <summary>
        /// Used in Analysis, we reverse the compilation by going back to Lexemes
        /// </summary>
        /// <param name="grammarNodeD"></param>
        /// <returns></returns>
        public List<Lexeme> Reverse(GrammarNodeD grammarNodeD)
        {
            if (grammarNodeD is OperationD)
                return Reverse(grammarNodeD as OperationD);

            List<Lexeme> returnVal = new List<Lexeme>();
            if (grammarNodeD is TerminalD)
            {
                returnVal.Add(Reverse((TerminalD)grammarNodeD));
                return returnVal;
            }

            if (grammarNodeD is NonterminalTailD)
            {
                returnVal.Add(Reverse((NonterminalTailD)grammarNodeD));
                return returnVal;
            }

            throw new NotImplementedException();
        }

        private List<Lexeme> Reverse(OperationD operationD)
        {
            List<Lexeme> returnVal = new List<Lexeme>();

            for (int i = 0; i < operationD.Children.Count; i++)
            {
                returnVal.AddRange(Reverse(operationD.Children[i]));

                if (i != operationD.Children.Count - 1)
                {
                    // not last
                    if (operationD is ChoiceD)
                        returnVal.Add(new Lexeme(LexemeType.Choice, -1));
                    else if (operationD is ConcatenationD)
                        returnVal.Add(new Lexeme(LexemeType.Concatenation, -1));
                    else
                        throw new NotImplementedException();
                }

            }

            return returnVal;
        }

        private Lexeme Reverse(TerminalD terminal)
        {
            return new Lexeme(LexemeType.Terminal, terminal.Name, -1);
        }

        private Lexeme Reverse(NonterminalTailD nont)
        {
            return new Lexeme(LexemeType.Nonterminal, nont.Reference.Name, -1);
        }
    }
}
