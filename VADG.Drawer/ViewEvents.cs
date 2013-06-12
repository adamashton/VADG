using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.Global;

namespace VADG.Drawer
{
    /// <summary>
    /// This class is responsible for firing events such that the GUI controls listen to and update on the fly.
    /// </summary>
    public class ViewEvents
    {
        public delegate void TerminalEventHandler(object sender, GrammarEventArgs e);
        /// <summary>
        /// The main event when something happens to a nonterminal expanded.
        /// </summary>
        public event TerminalEventHandler TerminalEvent;


        internal void Rename(Terminal terminalControl, string newName)
        {
            GrammarEventArgs eventArgs = new GrammarEventArgs();
            eventArgs.NewName = newName;
            eventArgs.Reference = terminalControl.TerminalV;
            eventArgs.Action = GrammarActions.Rename;

            if (TerminalEvent != null)
            {
                TerminalEvent(terminalControl, eventArgs);
            }
        }

        internal void Select(Terminal terminalControl)
        {
            GrammarEventArgs eventArgs = new GrammarEventArgs();
            eventArgs.Action = GrammarActions.Click;
            eventArgs.Reference = terminalControl.TerminalV;

            if (TerminalEvent != null)
            {
                TerminalEvent(terminalControl, eventArgs);
            }
        }
    }
}
