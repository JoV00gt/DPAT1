using DPAT1.Interfaces;

namespace DPAT1
{
    public class FSM
    {
        public List<IState> Children { get; set; }
        public List<Transition> Transitions { get; set; }
        public List<Trigger> triggers { get; set; }
        public List<Action> actions { get; set; }

        public FSM()
        {
            Children = new List<IState>();
            Transitions = new List<Transition>();
            triggers = new List<Trigger>();
            actions = new List<Action>();
        }

        public void AddAction(Action action)
        {
            actions.Add(action);
        }

        public void AddState(IState state)
        {
           Children.Add(state);
        }

        public void AddTransition(Transition transition)
        {
            Transitions.Add(transition);
        }

        public void AddTrigger(Trigger trigger)
        {
            triggers.Add(trigger);
        }
    }
}