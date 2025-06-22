using DPAT1.Enums;
using DPAT1.Interfaces;
using DPAT1.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = DPAT1.Action;

namespace DPAT1
{
    public class FSMBuilder : IBuilder
    {

        private FSM fsm;

        private readonly Dictionary<string, IState> _states = new();
        private readonly Dictionary<string, Trigger> _triggers = new();
        private readonly Dictionary<string, Action> _actions = new();
        private readonly Dictionary<string, Transition> _transitions = new();

        public FSMBuilder()
        {
            fsm = new FSM();
        }

        public Action? AddAction(string id, string description, string actionTypeString)
        {
            if (!Enum.TryParse<ActionType>(actionTypeString, out var actionType))
            {
                return null;
            }

            if (_actions.ContainsKey(id))
            {
                return null;
            }

            var action = new Action
            {
                Id = id,
                Description = description,
                Type = actionType
            };

            _actions[id] = action;
            return action;
        }


        public IState? AddState(string id, string parent, string description, StateType type)
        {
            if (_states.ContainsKey(id))
            {
                return null;
            }

            IState? state = type switch
            {
                StateType.INITIAL => new InitialState
                {
                    Id = id,
                    Name = description,
                    Type = StateType.INITIAL,
                    Transitions = new List<Transition>()
                },
                StateType.FINAL => new FinalState
                {
                    Id = id,
                    Name = description,
                    Type = StateType.FINAL,
                    Transitions = new List<Transition>()
                },
                StateType.SIMPLE => new SimpleState
                {
                    Id = id,
                    Name = description,
                    Type = StateType.SIMPLE,
                    Transitions = new List<Transition>(),
                    Actions = new List<Action>()
                },
                StateType.COMPOUND => new CompoundState
                {
                    Id = id,
                    Name = description,
                    Type = StateType.COMPOUND,
                    Transitions = new List<Transition>(),
                    Children = new List<IState>()
                },
                _ => null
            };

            if (state == null)
            {
                return null;
            }

            _states[id] = state;

            if (parent != "_" && _states.ContainsKey(parent))
            {
                var parentState = _states[parent];
                if (parentState is CompoundState compoundParent)
                {
                    compoundParent.AddChild(state); 
                }
            }
            return state;
        }

        public Transition? AddTransition(string id, string from, string to, string? triggerNameOrGuard, string? guardFromParser)
        {
            if (!_states.TryGetValue(from, out var source))
            {
                return null;
            }

            if (!_states.TryGetValue(to, out var target))
            {
                return null;
            }

            Trigger? trigger = null;
            string? guard = guardFromParser;

            if (!string.IsNullOrWhiteSpace(triggerNameOrGuard))
            {
                if (_triggers.TryGetValue(triggerNameOrGuard, out var foundTrigger))
                {
                    trigger = foundTrigger;
                }
                else
                {
                    guard = triggerNameOrGuard;
                }
            }

            Action? effect = null;
            if (_actions.TryGetValue(id, out var action))
            {
                effect = action;
            }

            var transition = new Transition(id, source, target, trigger, guard, effect);
            _transitions[id] = transition;
                
            return transition;
        }

        public Trigger? AddTrigger(string id, string description)
        {
            if (_triggers.ContainsKey(id))
            {
                return null;
            }

            var trigger = new Trigger
            {
                Id = id,
                Description = description
            };

            _triggers[id] = trigger;
            return trigger;
        }

        public void ConnectActionsToStates(Dictionary<string, IState> states, Dictionary<string, Action> actions)
        {
            foreach (var action in actions.Values)
            {
                foreach (var state in states.Values)
                {
                    if (state is SimpleState simpleState)
                    {
                        if (action.Id == state.Id)
                        {
                            simpleState.AddAction(action);
                        }
                    }
                }
            }
        }



        public FSM GetFSM()
        {
            return fsm;
        }
    }
}
