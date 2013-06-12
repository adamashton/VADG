using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.Global
{
    public enum SymbolType
    {
        Terminal = 0,
        Nonterminal = 1,
        Undefined = 2
    }

    //public enum UndefinedActions
    //{
    //    Replace = 0
    //}

    //public enum TerminalActions
    //{
    //    Rename = 0,
    //    Click = 1,
    //    Delete = 2,
    //    AddAfter = 3,
    //    AddBefore = 4
    //}

    public enum GrammarActions
    {
        Collapse = 0,
        Expand = 1,
        Flip = 2,
        Click = 3,
        Rename = 4,
        AddAfter = 5,
        AddBefore = 6,
        Delete = 7,
        Replace = 8,
        AddChoice = 9,
        RemoveChoice = 10,
        DoubleClick = 11
    }


}
