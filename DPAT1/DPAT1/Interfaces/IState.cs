using DPAT1.Enums;

namespace DPAT1.Interfaces
{
    public interface IState
    {
        string Id { get; set; }
        string Name { get; set; }
        StateType Type { get; set; }


        void AddTransition(Transition transition);
        List<Transition> GetTransitions();
        void ExecuteEntryAction();
        void ExecuteExitAction();

        void ExecuteDoAction();
        string GetName();
    }
}