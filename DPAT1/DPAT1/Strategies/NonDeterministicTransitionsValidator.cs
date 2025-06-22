using DPAT1.Helpers;
using DPAT1.Interfaces;

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
            var automaticTransitions = GetAutomaticTransitions(transitions);

            if (!HasValidUnguardedAutomaticTransitions(automaticTransitions, transitions))
                return false;

            if (!HasValidGuardedAutomaticTransitions(automaticTransitions))
                return false;

            return true;
        }

        private List<Transition> GetAutomaticTransitions(List<Transition> transitions)
        {
            return transitions.Where(t => t.Trigger == null).ToList();
        }

        private bool HasValidUnguardedAutomaticTransitions(List<Transition> automaticTransitions, List<Transition> allTransitions)
        {
            var automaticWithoutGuards = automaticTransitions.Where(t => string.IsNullOrEmpty(t.Guard)).ToList();

            if (automaticWithoutGuards.Count > FSMValidationRules.MAX_AUTOMATIC_TRANSITIONS_PER_STATE)
                return false;
  
            if (automaticWithoutGuards.Count > FSMValidationRules.NO_TRANSITIONS &&
                allTransitions.Count > automaticWithoutGuards.Count)
                return false;

            return true;
        }

        private bool HasValidGuardedAutomaticTransitions(List<Transition> automaticTransitions)
        {
            var automaticWithGuards = automaticTransitions.Where(t => !string.IsNullOrEmpty(t.Guard)).ToList();

            if (automaticWithGuards.Count >= FSMValidationRules.MIN_GUARDS_UNIQUENESS_CHECK)
            {
                return HaveUniqueGuards(automaticWithGuards);
            }

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