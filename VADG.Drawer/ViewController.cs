using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VADG.Drawer
{
    /// <summary>
    /// Used to hold the event listener and 'firer' of the View
    /// </summary>
    public static class ViewController
    {
        private static ViewListener viewListener;
        private static ViewEvents viewEvents;

        public static ViewListener ViewListener
        {
            get
            {
                if (viewListener == null)
                    viewListener = new ViewListener();

                return viewListener;
            }
        }

        public static ViewEvents ViewEvents
        {
            get
            {
                if (viewEvents == null)
                    viewEvents = new ViewEvents();

                return viewEvents;
            }
        }

        /// <summary>
        /// Reinitialise the controllers, this MUST be done before you redraw the view [everytime!]
        /// </summary>
        public static void ReInit()
        {
            viewEvents = null;
            viewListener = null;
        }
    }
}
