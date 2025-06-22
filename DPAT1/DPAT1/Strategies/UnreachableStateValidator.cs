using DPAT1.Enums;
using DPAT1.Interfaces;
using DPAT1.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPAT1.Strategies
{
    public class UnreachableStateValidator : IFSMValidator
    {
        public bool IsValid(FSM fsm)
        {
            bool hasInitialState = fsm.Children.Any(state => state.Type == StateType.INITIAL);

            if (!hasInitialState)
            {
                var unreachableStates = fsm.Children
                    .Where(state => state.Type != StateType.INITIAL && !IsStateReachable(state, fsm))
                    .ToList();

                if (unreachableStates.Count == 1 && unreachableStates[0].Type == StateType.SIMPLE)
                    return true;

                return unreachableStates.Count == 0;
            }

            return fsm.Children
                .Where(state => state.Type != StateType.INITIAL)
                .All(state => IsStateReachable(state, fsm));
        }

        private bool IsStateReachable(IState state, FSM fsm)
        {
            if (fsm.Transitions.Any(t => t.GetTarget() == state))
                return true;

            if (state.Type == StateType.COMPOUND)
            {
                var compoundState = (CompoundState)state;
                return compoundState.GetChildren().Any(child => IsStateReachable(child, fsm));
            }

            return false;
        }
    }
}