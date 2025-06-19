using DPAT1.Enums;
using DPAT1.Interfaces;

namespace DPAT1.States
{
    internal class CompoundState : IState
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public StateType Type { get; set; }
        private List<Transition> transitions = new List<Transition>();
        private List<IState> children = new List<IState>();

        public void AddTransition(Transition transition)
        {
            transition.Add(transition);
        }

        public void ExecuteEntryAction()
        {
            Console.WriteLine($"Entering compound state: {Name}");
            foreach(var child in children)
            {
                child.ExecuteEntryAction();
            }
        }

        public void ExecuteExitAction()
        {
            Console.WriteLine($"Entering compound state: {Name}");
            foreach (var child in children)
            {
                child.ExecuteExitAction();
            }
        }

        public void ExecuteDoAction()
        {
            Console.WriteLine($"Entering compound state: {Name}");
            foreach (var child in children)
            {
                child.ExecuteDoAction();
            }
        }

        public string GetName()
        {
            return Name;
        }

        public List<Transition> GetTransitions()
        {
            var allTransitions = new List<Transition>(transitions);

            foreach (var child in children)
            {
                allTransitions.AddRange(child.GetTransitions());
            }
            return allTransitions;
        }

        internal void Add(IState state)
        {
            throw new NotImplementedException();
        }
    }
}