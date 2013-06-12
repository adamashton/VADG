using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    /// <summary>
    /// visitor pattern, see http://en.wikipedia.org/wiki/Visitor_pattern
    /// </summary> 
    public interface GrammarNodeD_Visitor
    {
        //void visit(GrammarNodeD grammarNodeD);
        void visit(NonterminalHeadD nonterminalHeadD);
        void visit(NonterminalTailD nonterminalTailD);
        void visit(TerminalD terminalD);
        void visit(ConcatenationD concatenationD);
        void visit(ChoiceD choiceD);
        void visit(ChoiceLineD choiceLineD);
        void visit(UndefinedD undefinedD);

    }
}
