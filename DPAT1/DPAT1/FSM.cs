using System.Linq.Expressions;
using DPAT1.Interfaces;
using Action = DPAT1.Action;

namespace DPAT1
{
    public class FSM
    {
        private List<IState> states;
        private List<Transition> transitions;
        private List<Trigger> triggers;
        private List<Action> actions;

        public FSM()
        {
            states = new List<IState>();
            transitions = new List<Transition>();
            triggers = new List<Trigger>();
            actions = new List<Action>();
        }

        public void AddAction(Action action)
        {
            actions.Add(action);
        }

        public void AddState(IState state)
        {
           states.Add(state);
        }

        public void AddTransition(Transition transition)
        {
            transitions.Add(transition);
        }

        public void AddTrigger(Trigger trigger)
        {
            triggers.Add(trigger);
        }

        public Action GetActionById(string id)
        {
            return actions.Find(a => a.Id == id);
        }

        public IState GetStateById(string id)
        {
           return states.Find(s => s.Id == id);
        }

        public Trigger GetTriggerById(string id)
        {
            return triggers.Find(t => t.Id == id);
        }

        public void Accept(Visitor visitor)
        {
            //TODO: implementation of the visitor pattern
        }
    }
}