using DPAT1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPAT1.View
{
    public class ConsoleRenderer : IOutputRenderer
    {
        public FSM Fsm { get; }

        public ConsoleRenderer(FSM fsm)
        {
            Fsm = fsm;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
