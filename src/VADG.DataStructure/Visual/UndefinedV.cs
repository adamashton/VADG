using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public class UndefinedV : SymbolV
    {
        public UndefinedV(UndefinedD undefinedD, int levelIn)
        {
            reference = undefinedD;
            level = levelIn;            
        }
        
        private UndefinedD reference;
        /// <summary>
        /// A reference to the terminal class in the D grammar.
        /// </summary>
        public UndefinedD Reference
        {
            get { return reference; }
        }

        

        public override void Dispose()
        {
            // remove reference from undefinedD
            reference.References.Remove(this);

            // 
        }

        public override void accept(GrammarNodeV_Visitor visitor)
        {
            visitor.visit(this);
        }

        public override GrammarNodeD getReference()
        {
            return reference;
        }
    }
}
