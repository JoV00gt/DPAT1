﻿using DPAT1.Enums;


namespace DPAT1.Interfaces
{
    public interface IBuilder
    {
        IState? AddState(string id, string parent, string description, StateType type);
        Transition? AddTransition(string id, string sourceState, string targetState, string trigger, string guard);
        Trigger? AddTrigger(string id, string description);
        Action? AddAction(string id, string description, string actionType);
        void ConnectActionsToStates(Dictionary<string, IState> states, Dictionary<string, Action> actions);
        FSM GetFSM();
    }
}
