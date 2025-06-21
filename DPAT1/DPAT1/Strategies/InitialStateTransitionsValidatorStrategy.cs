using DPAT1.Enums;
using DPAT1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPAT1.Strategies
{
    public class InitialStateTransitionsValidatorStrategy : IFSMValidator
    {
        public bool IsValid(FSM fsm)
        {
            var initialStates = fsm.Children.Where(state => state.Type == StateType.INITIAL);

            return initialStates.All(initialState => !fsm.Transitions.Any(transition => transition.GetTarget() == initialState));
        }      
    }
}
