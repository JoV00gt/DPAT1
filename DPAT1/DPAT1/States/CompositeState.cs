using DPAT1.Interfaces;

namespace DPAT1.States
{
    internal class CompositeState : IState
    {
        private string id;
        private string name;

        public CompositeState(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public void AddTransition(Transition transition)
        {
            throw new NotImplementedException();
        }

        public void ExecuteEntryAction()
        {
            throw new NotImplementedException();
        }

        public void ExecuteExitAction()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public List<Transition> GetTransitions()
        {
            throw new NotImplementedException();
        }

        internal void Add(IState state)
        {
            throw new NotImplementedException();
        }
    }
}