using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public abstract class SymbolV : GrammarNodeV
    {
        protected int level;
        public int Level
        {
            get { return level; }
            set { level = value; }
        }
    }
}
