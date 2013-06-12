using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace VADG.Drawer
{
    /// <summary>
    /// This class registers for all the events that the view may call and makes them public such that the Controller
    /// can register for the events. It essentially just relays events. It will put them in a nice format, probably.
    /// </summary>
    public class ViewListener
    {
        public delegate void MyEventHandler(object sender, GrammarEventArgs e);
        /// <summary>
        /// The main event when something happens to a nonterminal collapsed.
        /// </summary>
        public event MyEventHandler NonterminalCollapsedControlEvent;
        public event MyEventHandler NonterminalExpandedControlEvent;
        public event MyEventHandler TerminalControlEvent;
        public event MyEventHandler UndefinedControlEvent;
        //public  event EventHandler ConcatenationControlEvent;
        // ...


        internal void Listen(NonterminalCollapsed nonterminalCollapsed)
        {
            nonterminalCollapsed.ControlEvent += new NonterminalCollapsed.ControlEventHandler(nonterminalCollapsed_ControlEvent);
        }

        internal void nonterminalCollapsed_ControlEvent(object sender, GrammarEventArgs e)
        {
            if (NonterminalCollapsedControlEvent != null)
                NonterminalCollapsedControlEvent(sender, e);
        }


        internal void Listen(NonterminalExpanded nonterminalExpanded)
        {
            nonterminalExpanded.ControlEvent += new NonterminalExpanded.ControlEventHandler(nonterminalExpanded_ControlEvent);
        }

        internal void nonterminalExpanded_ControlEvent(object sender, GrammarEventArgs e)
        {
            if (NonterminalExpandedControlEvent != null)
                NonterminalExpandedControlEvent(sender, e);
        }

        internal void Listen(Terminal terminal)
        {
            terminal.ControlEvent += new Terminal.ControlEventHandler(terminal_ControlEvent);
        }

        void terminal_ControlEvent(object sender, GrammarEventArgs e)
        {
            if (TerminalControlEvent != null)
                TerminalControlEvent(sender, e);
        }

        internal void Listen(Undefined undefined)
        {
            undefined.ControlEvent += new Undefined.ControlEventHandler(undefined_ControlEvent);
        }

        void undefined_ControlEvent(object sender, GrammarEventArgs e)
        {
            if (UndefinedControlEvent != null)
                UndefinedControlEvent(sender, e);
        }
    }
}
