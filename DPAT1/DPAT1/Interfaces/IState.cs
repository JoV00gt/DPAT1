using DPAT1.Enums;

namespace DPAT1.Interfaces
{
    public interface IState
    {
        string Id { get; set; }
        string Name { get; set; }

        List<Transition> Transitions { get; set; }

        void AddTransition(Transition transition);
        List<Transition> GetTransitions();
        string GetName();
    }
}