using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public class UndefinedD : SymbolD
    {
        public UndefinedD(GrammarNodeD parentIn)
        {
            references = new List<UndefinedV>();
            parent = parentIn;
        }
        
        private List<UndefinedV> references;
        /// <summary>
        /// A list of where this undefined is used in the visualised grammar
        /// </summary>
        public List<UndefinedV> References
        {
            get { return references; }
        }

        public override void accept(GrammarNodeD_Visitor visitor)
        {
            visitor.visit(this);
        }

        public override void Dispose()
        {
            parent = null;

            for (int i = 0; i < references.Count; i++)
                references[i].Dispose();

            references.Clear();
        }
    }
}
