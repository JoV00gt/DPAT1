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
                List<Transition> outgoingTransitions = GetOutgoingTransition(state, fsm);

                if (!AreTransitionsDeterministic(outgoingTransitions))
                    return false;
            }
            return true;
        }

        private bool AreTransitionsDeterministic(List<Transition> transitions)
        {
            var automaticTransitions = transitions.Where(t => t.Trigger == null).ToList();

            if (automaticTransitions.Count > 1)
                return false;

            if(automaticTransitions.Count > 0 && transitions.Count > 1)
                return false;

            var triggeredTransitions = transitions.Where(t => t.Trigger != null);
            var triggerGroups = triggeredTransitions.GroupBy(t => t.Trigger);

            foreach (var triggerGroup in triggerGroups)
            {
                if(triggerGroup.Count() > 1)
                {
                    var transitionsWithSameTrigger = triggerGroup.ToList();

                    if (transitionsWithSameTrigger.Any(t => t.Guard == null))
                        return false;


                    if (!HaveUniqueGuards(transitionsWithSameTrigger))
                        return false;
                }
            }

            return true;
        }

        private bool HaveUniqueGuards(List<Transition> transitions)
        {
            var guardConditions = transitions
                .Select(t => t.GetGuard())
                .ToList();

            return guardConditions.Distinct().Count() == guardConditions.Count();
        }

        private List<Transition> GetOutgoingTransition(IState state, FSM fsm)
        {
            return fsm.Transitions
                .Where(t => t.Source == state)
                .ToList();
        }
    }
}
