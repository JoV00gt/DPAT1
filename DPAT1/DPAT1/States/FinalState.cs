using DPAT1.Enums;
using DPAT1.Interfaces;

namespace DPAT1.States
{
    public class FinalState : IState
    {

        public string Name { get; set; }
        public StateType Type { get; set; }
        public string Id { get; set; }
        private List<Transition> transitions = new List<Transition>();

        public void AddTransition(Transition transition)
        {
            transition.Add(transition);
        }

        public void ExecuteEntryAction()
        {
            Console.WriteLine($"Entering final state: {Name}");
        }

        public void ExecuteExitAction()
        {
            Console.WriteLine($"Exiting final state: {Name}");
        }

        public string GetName()
        {
            return Name;
        }

        public List<Transition> GetTransitions()
        {
            return new List<Transition>(transitions);
        }

        public void ExecuteDoAction()
        {
            throw new NotImplementedException();
        }
    }
}