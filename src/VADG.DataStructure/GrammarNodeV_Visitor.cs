using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    /// <summary>
    /// visitor pattern, see http://en.wikipedia.org/wiki/Visitor_pattern
    /// </summary> 
    public interface GrammarNodeV_Visitor
    {
        //void visit(GrammarNodeV grammarNodeV);
        void visit(NonterminalHeadV nonterminalHeadV);
        void visit(NonterminalTailV nonterminalTailV);
        void visit(TerminalV terminalV);
        void visit(ConcatenationV concatenationV);
        void visit(ChoiceV choiceV);
        void visit(ChoiceLineV choiceLineV);
        void visit(UndefinedV undefinedV);

    }
}
