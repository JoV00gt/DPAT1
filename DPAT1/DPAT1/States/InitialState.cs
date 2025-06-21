using DPAT1.Enums;
using DPAT1.Interfaces;

namespace DPAT1.States
{
    public class InitialState : IState
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Transition> Transitions { get; set; }
        public StateType Type { get; set; }

        public void AddTransition(Transition transition)
        {
            Transitions.Add(transition);
        }

        public string GetName()
        {
            return Name;
        }

        public StateType GetStateType()
        {
            return Type;
        }

        public List<Transition> GetTransitions()
        {
            return Transitions;
        }
    }
}