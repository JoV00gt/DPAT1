using DPAT1.Enums;
using DPAT1.Helpers;
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
                return ValidateFSMWithoutInitialState(fsm);

            return ValidateFSMWithInitialState(fsm);
        }

        private bool ValidateFSMWithoutInitialState(FSM fsm)
        {
            var unreachableStates = GetUnreachableStates(fsm);

            if (IsValidImplicitInitialState(unreachableStates))
                return true;

            return unreachableStates.Count == FSMValidationRules.NO_UNREACHABLE_STATES;
        }

        private bool ValidateFSMWithInitialState(FSM fsm)
        {
            return fsm.Children
                .Where(state => state.Type != StateType.INITIAL)
                .All(state => IsStateReachable(state, fsm));
        }

        private List<IState> GetUnreachableStates(FSM fsm)
        {
            return fsm.Children
                .Where(state => state.Type != StateType.INITIAL && !IsStateReachable(state, fsm))
                .ToList();
        }

        private bool IsValidImplicitInitialState(List<IState> unreachableStates)
        {
            return unreachableStates.Count == FSMValidationRules.SINGLE_UNREACHABLE_STATE &&
                   unreachableStates[FSMValidationRules.FIRST_ELEMENT_INDEX].Type == StateType.SIMPLE;
        }

        private bool IsStateReachable(IState state, FSM fsm)
        {
            if (fsm.Transitions.Any(t => t.GetTarget() == state))
                return true;

            if (state.Type == StateType.COMPOUND)
                return IsCompoundStateReachable(state, fsm);

            return false;
        }

        private bool IsCompoundStateReachable(IState state, FSM fsm)
        {
            var compoundState = (CompoundState)state;
            return compoundState.GetChildren().Any(child => IsStateReachable(child, fsm));
        }
    }
}