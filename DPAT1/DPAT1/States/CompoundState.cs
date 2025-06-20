using DPAT1.Enums;
using DPAT1.Interfaces;

namespace DPAT1.States
{
    public class CompoundState : IState
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Transition> Transitions { get; set; }
        public List<IState> Children { get; set; }

        public void AddTransition(Transition transition)
        {
            Transitions.Add(transition);
        }

        public List<IState> GetChildren()
        {
            return Children;
        }

        public string GetName()
        {
            return Name;
        }

        public List<Transition> GetTransitions()
        {
            return Transitions;
        }
    }
}