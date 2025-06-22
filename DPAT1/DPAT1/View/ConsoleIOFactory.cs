using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPAT1.Interfaces;

namespace DPAT1.View
{
    public class ConsoleIOFactory : IUIFactory
    {
        private readonly FSM fsm;

        public ConsoleIOFactory(FSM fsm)
        {
            this.fsm = fsm;
        }

        public IOutputRenderer CreateRenderer()
        {
            return new ConsoleRenderer(fsm);
        }

        public IVisitor CreateVisitor()
        {
            return new PrintVisitor();
        }
    }
}
