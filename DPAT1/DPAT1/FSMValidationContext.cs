using DPAT1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPAT1
{
    public class FSMValidationContext
    {
        private IFSMValidator strategy;

        public void SetStrategy(IFSMValidator strategy)
        {
            this.strategy = strategy;
        }

        public bool Validate(FSM fsm)
        {
            return strategy.IsValid(fsm);
        }
    }
}
