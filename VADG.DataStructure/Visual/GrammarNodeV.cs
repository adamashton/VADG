using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public abstract class GrammarNodeV
    {
        protected Boolean direction = false;        
        /// <summary>
        /// if direction==false then draw L->R. if direction==true draw T->B.
        /// </summary>
        public Boolean Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public abstract void Dispose();        

        //internal abstract void ExpandAllNonterminalsOnce(List<NonterminalHeadD> seen);

        public abstract void accept(GrammarNodeV_Visitor visitor);

        public abstract GrammarNodeD getReference();
    }
}
