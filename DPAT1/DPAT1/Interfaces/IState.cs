namespace DPAT1.Interfaces
{
    public interface IState
    {
        string Id { get; }

        void AddTransition(Transition transition);
        List<Transition> GetTransitions();
        void ExecuteEntryAction();
        void ExecuteExitAction();
        string GetName();
    }
}