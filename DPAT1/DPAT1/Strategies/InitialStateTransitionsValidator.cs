using DPAT1.Enums;
using DPAT1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPAT1.Strategies
{
    public class InitialStateTransitionsValidator : IFSMValidator
    {
        public bool IsValid(FSM fsm)
        {
            var initialStates = GetInitialStates(fsm);

            return initialStates.All(initialState => HasNoIncomingTransitions(initialState, fsm));
        }

        private IEnumerable<IState> GetInitialStates(FSM fsm)
        {
            return fsm.Children.Where(state => state.Type == StateType.INITIAL);
        }

        private bool HasNoIncomingTransitions(IState initialState, FSM fsm)
        {
            return !fsm.Transitions.Any(transition => transition.GetTarget() == initialState);
        }
    }
}