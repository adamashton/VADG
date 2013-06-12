using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public class TerminalD : SymbolD
    {
        #region Variables

        private List<TerminalV> references;
        /// <summary>
        /// A list of where this terminal is used in the visualised grammar
        /// </summary>
        public List<TerminalV> References
        {
            get { return references; }
        }

        private String name;
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion        

        #region Constructors
        
        public TerminalD(String nameIn, GrammarNodeD parentIn)
        {
            Init(nameIn, parentIn);
        }

        private void Init(string nameIn, GrammarNodeD parentIn)
        {
            name = nameIn;
            references = new List<TerminalV>();
            parent = parentIn;
        } 
        #endregion

        #region Overrides
        //internal override GrammarNodeV CreateVisual(Boolean updateRef)
        //{
        //    TerminalV returnVal = new TerminalV(this);

        //    if (updateRef)
        //        references.Add(returnVal);

        //    return returnVal;
        //}

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

        public override string ToString()
        {
            return "'" + name + "'";
        }



        #endregion
    }
}
