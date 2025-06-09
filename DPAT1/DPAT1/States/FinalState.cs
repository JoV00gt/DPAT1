using DPAT1.Interfaces;

namespace DPAT1.States
{
    public class FinalState : IState
    {
        private string id;
        private string name;

        public FinalState(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public string Id => throw new NotImplementedException();

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
    }
}