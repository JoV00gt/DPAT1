using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPAT1.Interfaces
{
    public interface IBuilder
    {
        void AddState(string id, string name, string type, string parent);
        void AddTransition(string id, string sourceState, string targetState, string trigger, string guard);
        void AddTrigger(string id, string description);
        void AddAction(string id, string description, string actionType);
        void ConnectActionToState(string actionId, string stateId);
        FSM GetFSM();
    }
}
