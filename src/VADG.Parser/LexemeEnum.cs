using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.Parser
{
    public enum LexemeType
    {
        Assignment,
        Nonterminal,
        Terminal,
        Concatenation,
        Choice,
        EndOfRule,
        Commment,
        LineComment,
        EOF,
        Undefined
    }
}
