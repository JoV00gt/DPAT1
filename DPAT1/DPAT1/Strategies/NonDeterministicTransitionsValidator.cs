using DPAT1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPAT1.Strategies
{
    public class NonDeterministicTransitionsValidator : IFSMValidator
    {
        public bool IsValid(FSM fsm)
        {
            foreach (IState state in fsm.Children)
            {
                List<Transition> outgoingTransitions = GetOutgoingTransitions(state, fsm);

                if (!HasValidAutomaticTransitions(outgoingTransitions))
                    return false;

                if (!HasValidTriggeredTransitions(outgoingTransitions))
                    return false;
            }
            return true;
        }

        private bool HasValidAutomaticTransitions(List<Transition> transitions)
        {
            var automaticTransitions = transitions.Where(t => t.Trigger == null).ToList();

            if (automaticTransitions.Count > FSMValidationRules.MAX_AUTOMATIC_TRANSITIONS_PER_STATE)
                return false;

            if (automaticTransitions.Count > FSMValidationRules.NO_UNREACHABLE_STATES &&
                transitions.Count > FSMValidationRules.MAX_AUTOMATIC_TRANSITIONS_PER_STATE)
                return false;

            return true;
        }

        private bool HasValidTriggeredTransitions(List<Transition> transitions)
        {
            var triggeredTransitions = transitions.Where(t => t.Trigger != null);
            var triggerGroups = triggeredTransitions.GroupBy(t => t.Trigger);

            foreach (var triggerGroup in triggerGroups)
            {
                if (!IsValidTriggerGroup(triggerGroup.ToList()))
                    return false;
            }

            return true;
        }

        private bool IsValidTriggerGroup(List<Transition> transitionsWithSameTrigger)
        {
            if (transitionsWithSameTrigger.Count <= FSMValidationRules.MAX_AUTOMATIC_TRANSITIONS_PER_STATE)
                return true;

            if (transitionsWithSameTrigger.Any(t => t.Guard == null))
                return false;

            return HaveUniqueGuards(transitionsWithSameTrigger);
        }

        private bool HaveUniqueGuards(List<Transition> transitions)
        {
            var guardConditions = transitions
                .Select(t => t.GetGuard())
                .ToList();

            return guardConditions.Distinct().Count() == guardConditions.Count();
        }

        private List<Transition> GetOutgoingTransitions(IState state, FSM fsm)
        {
            return fsm.Transitions
                .Where(t => t.Source == state)
                .ToList();
        }
    }
}