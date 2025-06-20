using DPAT1.Interfaces;
using DPAT1.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = DPAT1.Action;

namespace DPAT1
{
    public class FSMBuilder : IBuilder
    {

        private FSM fsm;

        public FSMBuilder()
        {
            fsm = new FSM();
        }
        public void AddAction(string id, string description, string actionType)
        {
            Action action = null;
            
            switch(actionType.ToUpper())
            {
                case "ENTRY_ACTION":
                    action = new EntryAction(id, description);
                    break;
                case "DO_Action":
                    action = new DoAction(id, description);
                    break;
                case "EXIT_ACTION":
                    action = new ExitAction(id, description);
                    break;
                case "TRANSITION_ACTION":
                    action = new TransitionAction(id, description);
                    break;
                default:
                    throw new ArgumentException($"Unknown action type: {actionType}");
            }

            fsm.AddAction(action);

        }

        public void AddState(string id, string name, string type, string parent)
        {
            IState state = null;

            switch(type.ToUpper())
            {
                case "INITIAL":
                    state = new InitialState(id, name);
                    break;
                case "FINAL":
                    state = new FinalState(id, name);
                    break;
                case "SIMPLE":
                    state = new SimpleState(id, name);
                    break;
                case "COMPOUND":
                    state = new CompoundState(id, name);
                    break;
                default:
                    throw new ArgumentException($"Unknown state type: {type}");
            }

            if (parent != null && parent != "_")
            {
                IState parentState = fsm.GetStateById(parent);
                if(parentState is CompoundState compositeState)
                {
                    compositeState.Add(state);
                } 
                else
                {
                    throw new ArgumentException("Parent state must be a composite state");
                }
            }

            fsm.AddState(state);
        }

        public void AddTransition(string id, string sourceState, string targetState, string trigger, string guard)
        {
            IState source = fsm.GetStateById(sourceState);
            IState target = fsm.GetStateById(targetState);

            if(source != null || target != null)
            {
                throw new ArgumentException("Source or target State not found");
            }

            Transition transition = new Transition(id, source, target);

            if (!string.IsNullOrEmpty(trigger) && trigger != "_")
            {
                Trigger t = fsm.GetTriggerById(trigger); 
                if(t != null) 
                {
                    transition.SetTrigger(t);
                }
            }

            if(!string.IsNullOrEmpty(guard) && guard != "\"\"")
            {
                transition.SetGuardCondition(guard);
            }

            source.AddTransition(transition);
            fsm.AddTransition(transition);
        }

        public void AddTrigger(string id, string description)
        {
            Trigger trigger = new Trigger(id, description);
            fsm.AddTrigger(trigger);
        }

        public void ConnectActionToState(string actionId, string stateId)
        {
            Action action = fsm.GetActionById(actionId);
            IState state = fsm.GetStateById(stateId);

            if(action == null || state == null)
            {
                throw new ArgumentException("Action or state not found");
            }

            if(action is EntryAction entryAction)
            {
                //TODO: connect entry action to state

            }
            else if (action is DoAction doAction)
            {
                //TODO: connect do action to state
            }
            else if (action is ExitAction exitAction)
            {
                //TODO: connect exit action to state
            }
        }

        public FSM GetFSM()
        {
            return fsm;
        }
    }
}
