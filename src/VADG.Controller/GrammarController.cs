using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VADG.Model;
using VADG.Drawer;
using VADG.DataStructure;
using VADG.Global;

namespace VADG.Controller
{
    /// <summary>
    /// Listens to events in the view, controls to call methods in the model and the view
    /// </summary>
    public class GrammarController
    {
        private GrammarModel model;
        private View view;

        private TerminalActionBar tAb;

        private NonterminalActionBar NontAb;

        #region Initialisation and Event delegates
        
        public GrammarController(GrammarModel modelIn, View viewIn)
        {
            model = modelIn;
            view = viewIn;

            // register for events from the viewListener
            ViewController.ViewListener.NonterminalCollapsedControlEvent +=new ViewListener.MyEventHandler(NonterminalCollapsedControlEvent);
            ViewController.ViewListener.NonterminalExpandedControlEvent += new ViewListener.MyEventHandler(NonterminalExpandedControlEvent);
            ViewController.ViewListener.TerminalControlEvent += new ViewListener.MyEventHandler(TerminalControlEvent);
            ViewController.ViewListener.UndefinedControlEvent += new ViewListener.MyEventHandler(UndefinedControlEvent);
        }

        void UndefinedControlEvent(object sender, GrammarEventArgs eventArgs)
        {
            try
            {
                if (sender is Undefined)
                {
                    Undefined control = (Undefined)sender;

                    switch (eventArgs.Action)
                    {
                        case GrammarActions.Replace:
                            Replace(control, eventArgs);
                            break;
                        case GrammarActions.Delete:
                            Delete(eventArgs.Reference);
                            break;
                        case GrammarActions.AddAfter:
                            AddAfter(eventArgs);
                            break;
                        case GrammarActions.AddBefore:
                            AddBefore(eventArgs);
                            break;
                        case GrammarActions.AddChoice:
                            AddChoice(eventArgs);
                            break;
                        case GrammarActions.RemoveChoice:
                            RemoveChoice(eventArgs.Reference);
                            break;

                        default: throw new Exception("The action " + eventArgs.Action + " is not supported by the controller");
                    }
                }
            }
            catch (Exception e)
            {
                FireError(e);
            }
        }

        void TerminalControlEvent(object sender, GrammarEventArgs eventArgs)
        {
            try
            {
                if (sender is Terminal)
                {
                    Terminal control = (Terminal)sender;

                    switch (eventArgs.Action)
                    {
                        case GrammarActions.Rename:
                            Rename(control, eventArgs);
                            break;
                        case GrammarActions.Click:
                            SelectedTerminal(control, eventArgs);
                            break;
                        case GrammarActions.Delete:
                            Delete(eventArgs.Reference);
                            break;
                        case GrammarActions.AddAfter:
                            AddAfter(eventArgs);
                            break;
                        case GrammarActions.AddBefore:
                            AddBefore(eventArgs);
                            break;
                        case GrammarActions.AddChoice:
                            AddChoice(eventArgs);
                            break;
                        case GrammarActions.RemoveChoice:
                            RemoveChoice(eventArgs.Reference);
                            break;

                        default: throw new Exception("The action " + eventArgs.Action + " is not supported by the controller");
                    }
                }
            }
            catch (Exception e)
            {
                FireError(e);
            }        
        }       

        void NonterminalControlEvent(object sender, GrammarEventArgs e)
        {
            try
            {
                if (sender is NonterminalCollapsed)
                    NonterminalCollapsedControlEvent(sender, e);
                else if (sender is NonterminalExpanded)
                    NonterminalExpandedControlEvent(sender, e);
            }
            catch (Exception ex)
            {
                FireError(ex);
            }
        }

        void NonterminalExpandedControlEvent(object sender, GrammarEventArgs eventArgs)
        {
            // an event from an expanded nontermianl control
            if (sender is NonterminalExpanded)
            {
                NonterminalExpanded thisSender = (NonterminalExpanded)sender;                

                switch (eventArgs.Action)
                {
                    case GrammarActions.Collapse:
                        collapse(thisSender, eventArgs);
                        break;
                    case GrammarActions.Click:
                        SelectedNonterminal(thisSender, eventArgs);
                        break;
                    case GrammarActions.Rename:
                        RenameNonterminal(eventArgs);
                        break;
                    case GrammarActions.Delete:
                        Delete(eventArgs.Reference);
                        break;
                    case GrammarActions.AddAfter:
                        AddAfter(eventArgs);
                        break;
                    case GrammarActions.AddBefore:
                        AddBefore(eventArgs);
                        break;
                    case GrammarActions.AddChoice:
                        AddChoice(eventArgs);
                        break;
                    case GrammarActions.RemoveChoice:
                        RemoveChoice(eventArgs.Reference);
                        break;
                    case GrammarActions.DoubleClick:
                        DoubleClick(eventArgs.Reference);
                        break;
                    default: throw new Exception("The action " + eventArgs.Action + " is not supported by the nont expanded event handler");
                }
            }
        }

        private void NonterminalCollapsedControlEvent(object sender, GrammarEventArgs eventArgs)
        {
            // an event from a collapsed nonterminal control
            if (sender is NonterminalCollapsed)
            {
                NonterminalCollapsed thisSender = (NonterminalCollapsed)sender;                

                switch (eventArgs.Action)
                {
                    case GrammarActions.Expand:
                        expand(thisSender, eventArgs);
                        break;
                    case GrammarActions.Click:
                        SelectedNonterminal(thisSender, eventArgs);
                        break;
                    case GrammarActions.Rename:
                        RenameNonterminal(eventArgs);
                        break;
                    case GrammarActions.AddAfter:
                        AddAfter(eventArgs);
                        break;
                    case GrammarActions.AddBefore:
                        AddBefore(eventArgs);
                        break;
                    case GrammarActions.Delete:
                        Delete(eventArgs.Reference);
                        break;
                    case GrammarActions.AddChoice:
                        AddChoice(eventArgs);
                        break;
                    case GrammarActions.RemoveChoice:
                        RemoveChoice(eventArgs.Reference);
                        break;
                    case GrammarActions.DoubleClick:
                        DoubleClick(eventArgs.Reference);
                        break;

                    default: throw new Exception("The action " + eventArgs.Action + " is not supported by the nont collapsed event handler");
                }
            }
        }

        #endregion

        #region Actions

        private void expand(NonterminalCollapsed nonterminalCollapsed, GrammarEventArgs e)
        {
            lock (new System.Object())
            {
                // the reference to the tail v
                NonterminalTailV nonterminalTailV = (NonterminalTailV)e.Reference;

                if (!nonterminalTailV.IsExpanded)
                {
                    // expand the model (visualised grammar)
                    model.Functions.Expand(nonterminalTailV);

                    // expand the view
                    view.Functions.Expand(nonterminalCollapsed);                    
                }
            }
        
        }

        private void collapse(NonterminalExpanded nonterminalExpanded, GrammarEventArgs e)
        {
            lock (new System.Object())
            {
                NonterminalTailV nonterminalTailV = (NonterminalTailV)e.Reference;

                if (nonterminalTailV.IsExpanded)
                {
                    // collapse in the model
                    model.Functions.Collapse(nonterminalTailV);

                    // collapse in the view
                    view.Functions.Collapse(nonterminalExpanded);

                    // re draw the view - we do this to reinitialise the controllers - not needed when expanding
                    ReDraw();                             
                }
            }
        }

        void nontExpanded_Flipping(object sender, GrammarEventArgs e)
        {
            //if (sender is NonterminalExpanded)
            //{
            //    NonterminalExpanded nont = (NonterminalExpanded)sender;
            //    NonterminalTailV nonterminalTailV = (NonterminalTailV)e.Reference;

            //    nonterminalTailV.Flip();

            //    // draw control
            //    GrammarNodeVDrawVisitor drawer = new GrammarNodeVDrawVisitor();
            //    drawer.visit(nonterminalTailV);

            //    // replace child
            //    int index = e.MyParent.Children.IndexOf(nont);
            //    if (index < 0)
            //        throw new Exception("Cant find child in my stack panel, uh oh!");

            //    e.MyParent.Children.RemoveAt(index);
            //    e.MyParent.Children.Insert(index, drawer.DrawnItem);
            //}
        }

        private void Rename(Terminal terminalControl, GrammarEventArgs e)
        {
            TerminalV terminalV = e.Reference as TerminalV;

            if (terminalV == null)
                throw new Exception("The references visualised item is not of type TerminalV");

            // rename in the model
            model.Functions.Rename(terminalV, e.NewName);

            // rename in the view
            //view.Functions.Rename(terminalControl, e.NewName);
            
            // the redraw will cause the view to be renamed and updated
            ReDraw();
        }

        private void SelectedTerminal(Terminal control, GrammarEventArgs e)
        {
            UnselectAll();

            // set the terminal in the view to be selected
            control.TerminalV.Reference.IsSelected = true;

            ReDraw();

            if (ControlSelected != null)
            {
                ControlSelected(control, e);
            }
        }

        private void UnselectAll()
        {
            // unselect all
            GrammarNodeDUnselectVisitor visitor = new GrammarNodeDUnselectVisitor();
            foreach (NonterminalHeadD nont in model.DefinitiveGrammar.Nonterminals)
            {
                visitor.visit(nont);
            }
        }

        private void SelectedNonterminal(NonterminalExpanded control, GrammarEventArgs eventArgs)
        {
            SelectedNonterminal(eventArgs);

            if (ControlSelected != null)
            {
                ControlSelected(control, eventArgs);
            }
        }

        private void SelectedNonterminal(NonterminalCollapsed control, GrammarEventArgs eventArgs)
        {
            SelectedNonterminal(eventArgs);

            if (ControlSelected != null)
            {
                ControlSelected(control, eventArgs);
            }
        }

        private void SelectedNonterminal(GrammarEventArgs e)
        {
            UnselectAll();
            
            // set the rule to be selected
            if (e.Reference is NonterminalHeadV)
                (e.Reference as NonterminalHeadV).Reference.IsSelected = true;
            else if (e.Reference is NonterminalTailV)
                (e.Reference as NonterminalTailV).Reference.Reference.IsSelected = true;

            ReDraw();
        }

        private void RenameNonterminal(GrammarEventArgs eventArgs)
        {
            // get nontHead we're renaming
            NonterminalHeadD nont;
            if (eventArgs.Reference is NonterminalHeadV)
                nont = (eventArgs.Reference as NonterminalHeadV).Reference;
            else
                nont = (eventArgs.Reference as NonterminalTailV).Reference.Reference;

            if (model.getNonterminalNames().Contains(eventArgs.NewName))
            {
                // can't rename to nonterminal already in grammar
                throw new Exception("That nonterminal name already exists in the grammar, choose another one.");
            }

            // rename in model
            model.Functions.Rename(nont, eventArgs.NewName);

            // the redraw will cause the view to be updated
            ReDraw();            
        }

        private void AddBefore(GrammarEventArgs eventArgs)
        {
            // add a new undefined item in the model            
            UndefinedD undefinedD = model.Functions.AddBefore(eventArgs.Rule, eventArgs.Reference, SymbolType.Undefined) as UndefinedD;

            // traverse the view grammar and add this undefined property everywhere :o
            view.Functions.AddBefore(eventArgs.Reference, undefinedD);

            ReDraw();
        }

        private void AddAfter(GrammarEventArgs eventArgs)
        {
            // add a new undefined item in the model            
            UndefinedD undefinedD = model.Functions.AddAfter(eventArgs.Rule, eventArgs.Reference, SymbolType.Undefined) as UndefinedD;

            // traverse the view grammar and add this undefined property everywhere :o
            view.Functions.AddAfter(eventArgs.Reference, undefinedD);

            ReDraw();                      
        }

        private void Replace(Undefined control, GrammarEventArgs eventArgs)
        {
            GrammarNodeD itemToReplace = control.Reference.getReference();
            
            GrammarNodeD newItem = model.Functions.Replace(itemToReplace, eventArgs.SymbolType, eventArgs.NewName);

            view.Functions.Replace(itemToReplace, newItem);

            ReDraw();
        }

        private void Delete(GrammarNodeV itemToDelete)
        {
            model.Functions.Delete(itemToDelete.getReference());

            // if deleting a rule, check it's not the last time it's ever used (poor rule :( )
            GrammarNodeD grammarNodeD = itemToDelete.getReference();
            if (grammarNodeD is NonterminalTailD)
            {
                NonterminalTailD nontTail = grammarNodeD as NonterminalTailD;
                String ruleName = nontTail.Reference.Name;

                // traverse from the startsymbol through the grammar
                // if we never find this rule used, offer to remove the rule from the definitive grammar
                GrammarNodeDExistsVisitor visitor = new GrammarNodeDExistsVisitor(ruleName, VADG.Global.SymbolType.Nonterminal);
                model.DefinitiveGrammar.StartSymbol.accept(visitor);

                if (!visitor.Found)
                {
                    if (OfferDeleteRule != null)
                        OfferDeleteRule(nontTail.Reference);
                }
            }

            view.Functions.Delete(itemToDelete);

            ReDraw();
        }

        private void AddChoice(GrammarEventArgs eventArgs)
        {
            model.Functions.AddChoice(eventArgs.Reference.getReference());

            view.Functions.AddChoice(eventArgs.Reference);

            ReDraw();
        }

        private void RemoveChoice(SymbolV symbolV)
        {
            ConcatenationD concatD = symbolV.getReference().Parent as ConcatenationD;
            if (concatD == null)
                throw new Exception("The symbol does not have a parent of type concatenationD");

            model.Functions.Delete(concatD);

            view.Functions.Delete(concatD);

            ReDraw();
        }

        private void DoubleClick(SymbolV symbolV)
        {
            if (symbolV is NonterminalTailV)
            {
                NonterminalTailV nontTail = symbolV as NonterminalTailV;

                model.setStartSymbol(nontTail.Name);

                ReDraw();
            }
        }

        #endregion

        #region Events for the Window

        public delegate void ReDrawEventHandler(object sender, EventArgs e);
        public event ReDrawEventHandler RedrawEvent;

        /// <summary>
        /// Raises event such that the parent window will force a redrawing of the grammar and the text
        /// </summary>
        private void ReDraw()
        {
            if (RedrawEvent != null)
            {
                RedrawEvent(this, new EventArgs());
            }
        }

        public delegate void ControlSelectedHandler(object sender, GrammarEventArgs e);
        public event ControlSelectedHandler ControlSelected;

        public delegate void ErrorEventHandler(Exception exception);
        public event ErrorEventHandler ErrorSerious;
        private void FireErrorSerious(Exception e)
        {
            if (ErrorSerious != null)
            {
                ErrorSerious(e);
            }
        }

        public event ErrorEventHandler Error;
        private void FireError(Exception e)
        {
            if (Error != null)
            {
                Error(e);
            }
        }

        public delegate void OfferDeleteRuleHandler(NonterminalHeadD rule);
        public event OfferDeleteRuleHandler OfferDeleteRule;

        /// <summary>
        /// Raises event such that the parent window knows when a control has been selected to load the ActionBar
        /// Raises an event such that the view can set the terminals to be selected when a button is clicked.
        /// </summary>
        
        #endregion


        /// <summary>
        /// We register for events from this action bar
        /// </summary>
        /// <param name="tab"></param>
        
        
        public void setActionBar(TerminalActionBar tabIn)
        {
            Unregister();

            tAb = tabIn;

            tAb.ControlEvent += new TerminalActionBar.ControlEventHandler(TerminalControlEvent);
        }

        public void setActionBar(NonterminalActionBar tabIn)
        {
            Unregister();

            NontAb = tabIn;

            NontAb.ControlEvent += new NonterminalActionBar.ControlEventHandler(NonterminalControlEvent);
        }        

        private void Unregister()
        {
            if (NontAb != null)
                NontAb.ControlEvent -= new NonterminalActionBar.ControlEventHandler(NonterminalControlEvent);

            if (tAb != null)
                tAb.ControlEvent -= new TerminalActionBar.ControlEventHandler(TerminalControlEvent);
        }
    }
}
