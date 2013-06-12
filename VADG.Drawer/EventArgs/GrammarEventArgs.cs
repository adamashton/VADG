using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.Global;
using VADG.DataStructure;

namespace VADG.Drawer
{
    public class GrammarEventArgs : EventArgs
    {
        private GrammarActions action;
        /// <summary>
        /// The action to be performed on the control
        /// </summary>
        public GrammarActions Action
        {
            get { return action; }
            set { action = value; }
        }

        private SymbolType symbolType;

        public SymbolType SymbolType
        {
            get { return symbolType; }
            set { symbolType = value; }
        }
        
        private NonterminalHeadD rule;
        /// <summary>
        /// The Rule we are editing, if at all.
        /// </summary>
        public NonterminalHeadD Rule
        {
            get { return rule; }
        }

        internal void setRule(NonterminalHeadV nonterminalHeadV)
        {
            rule = nonterminalHeadV.Reference;
        }

        internal void setRule(NonterminalTailV nonterminalTailV)
        {
            rule = nonterminalTailV.Reference.getMyRule();
        }

        internal void setRule(TerminalV terminalV)
        {
            rule = terminalV.Reference.getMyRule();
        }

        internal void setRule(UndefinedV undefinedV)
        {
            rule = undefinedV.Reference.getMyRule();
        }

        private SymbolV reference;
        /// <summary>
        /// A Reference to the visualised grammar item
        /// </summary>
        public SymbolV Reference
        {
            get { return reference; }
            set { reference = value; }
        }

        private String newName;
        /// <summary>
        /// The new name of the control when it is renamed.
        /// </summary>
        public String NewName
        {
            get { return newName; }
            set { newName = value; }
        }
        
    }
}
