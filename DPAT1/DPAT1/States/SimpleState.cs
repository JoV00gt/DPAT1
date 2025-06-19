using DPAT1.Enums;
using DPAT1.Interfaces;

namespace DPAT1.States
{
    internal class SimpleState : IState
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public StateType Type { get; set; }
        private List<Transition> transitions = new List<Transition>();

        public void AddTransition(Transition transition)
        {
            transition.Add(transition);
        }

        public void ExecuteEntryAction()
        {
            Console.WriteLine($"Entering simple state: {Name}");
        }

        public void ExecuteExitAction()
        {
            Console.WriteLine($"Exiting simple state: {Name}");
        }

        public void ExecuteDoAction()
        {
            Console.WriteLine($"executing do action in simple state: {Name}");
        }

        public string GetName()
        {
            return Name;
        }

        public List<Transition> GetTransitions()
        {
            return new List<Transition>(transitions);
        }
    }
}