using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.DataStructure
{
    public class GrammarNodeDAddBeforeVisitor : GrammarNodeD_Visitor
    {
        private GrammarNodeD target;
        /// <summary>
        /// The Item we want to add the undefined before
        /// </summary>
        public GrammarNodeD Target
        {
            set { target = value; }
        }

        private GrammarNodeD item;
        /// <summary>
        /// THe item added to the grammar
        /// </summary>
        public GrammarNodeD Item
        {
            get { return item; }
        }

        private VADG.Global.SymbolType symbolType;
        public VADG.Global.SymbolType SymbolType
        {
            set { symbolType = value; }
        }

        public void visit(UndefinedD undefinedD)
        {

        }

        public void visit(TerminalD terminalD)
        {

        }

        public void visit(NonterminalHeadD nonterminalHeadD)
        {
            // visit rule where representation is done for each choiceLineD
            nonterminalHeadD.Rule.accept(this);
        }

        public void visit(NonterminalTailD nonterminalTailD)
        {

        }

        public void visit(ConcatenationD concatenationD)
        {
            int i = 0;
            int position = -1;
            foreach (GrammarNodeD node in concatenationD.Children)
            {
                if (node == target)
                {
                    position = i;
                    break;
                }
                node.accept(this);
                i++;
            }

            if (position > -1)
            {
                createItem(concatenationD);
                concatenationD.insertChild(position, item);
            }
        }

        public void visit(ChoiceLineD choiceLineD)
        {
            foreach (GrammarNodeD node in choiceLineD.Children)
                node.accept(this);
        }

        public void visit(ChoiceD choiceD)
        {
            foreach (GrammarNodeD node in choiceD.Children)
                node.accept(this);
        }

        private void createItem(GrammarNodeD parent)
        {
            GrammarNodeD newItem;

            switch (symbolType)
            {
                case VADG.Global.SymbolType.Undefined:
                    newItem = new UndefinedD(parent);
                    break;
                default: throw new Exception("The visitor does not support that type yet, get on it!");
            }

            item = newItem;
        }
    }
}
