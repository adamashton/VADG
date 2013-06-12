 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public class TerminalV : SymbolV
    {
        #region Variables
        private TerminalD reference;
        /// <summary>
        /// A reference to the terminal class in the D grammar.
        /// </summary>
        public TerminalD Reference
        {
            get { return reference; }
        }

        public String Name
        {
            get { return reference.Name; }
        }

        
        #endregion
        
        #region Constructors
        public TerminalV(TerminalD refIn, int levelIn)
        {
            Init(refIn, levelIn);
        }      

        private void Init(TerminalD refIn, int levelIn)
        {
            reference = refIn;
            level = levelIn;
        } 
        #endregion

        #region Overrides

        public override string ToString()
        {
            return VADG.Global.Settings.TerminalEncapsulation + Name + VADG.Global.Settings.TerminalEncapsulation;
        }

        public override void Dispose()
        {
            // remove reference from terminalD
            reference.References.Remove(this);

            // 
        }
        //internal override void ExpandAllNonterminalsOnce(List<NonterminalHeadD> seen)
        //{
        //    // nothing to do here
        //    return;
        //}

        public override void accept(GrammarNodeV_Visitor visitor)
        {
            visitor.visit(this);
        }

        public override GrammarNodeD getReference()
        {
            return reference;
        }
       
        #endregion
    }
}
