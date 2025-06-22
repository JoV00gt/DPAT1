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
                Console.WriteLine($"[AddAction] Unknown action type: '{actionTypeString}'");
                return null;
            }

            if (_actions.ContainsKey(id))
            {
                Console.WriteLine($"[AddAction] Action with ID '{id}' already exists. Skipping.");
                return null;
            }

            var action = new Action
            {
                Id = id,
                Description = description,
                Type = actionType
            };

            _actions[id] = action;

            Console.WriteLine($"[AddAction] Added action: Id='{id}', Description='{description}', Type='{actionType}'");

            return action;
        }

        // Optional: public method to retrieve actions
        public Action GetAction(string id) => _actions.TryGetValue(id, out var action) ? action : null;

        public IState? AddState(string id, string parent, string description, string type)
        {
            if (_states.ContainsKey(id))
            {
                Console.WriteLine($"[AddState] State with ID '{id}' already exists. Skipping.");
                return null;
            }

            IState? state = type switch
            {
                "INITIAL" => new InitialState
                {
                    Id = id,
                    Name = description,
                    Type = StateType.INITIAL,
                    Transitions = new List<Transition>()
                },
                "FINAL" => new FinalState
                {
                    Id = id,
                    Name = description,
                    Type = StateType.FINAL,
                    Transitions = new List<Transition>()
                },
                "SIMPLE" => new SimpleState
                {
                    Id = id,
                    Name = description,
                    Type = StateType.SIMPLE,
                    Transitions = new List<Transition>(),
                    Actions = new List<Action>()
                },
                "COMPOUND" => new CompoundState
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
                Console.WriteLine($"[AddState] Unknown state type: '{type}' for ID '{id}'. Skipping.");
                return null;
            }

            _states[id] = state;

            if (parent != "_" && _states.ContainsKey(parent))
            {
                var parentState = _states[parent];
                if (parentState is CompoundState compoundParent)
                {
                    compoundParent.AddChild(state); 
                    Console.WriteLine($"[AddState] Added {state.Name} as child of {parentState.Name}");
                }
            }

            Console.WriteLine($"[AddState] Added {type} state: Id='{id}', Name='{description}', Parent='{parent}'");
            return state;
        }

        public Transition? AddTransition(string id, string from, string to, string? triggerNameOrGuard, string? guardFromParser)
        {
            if (!_states.TryGetValue(from, out var source))
            {
                Console.WriteLine($"[AddTransition] Source state '{from}' not found.");
                return null;
            }

            if (!_states.TryGetValue(to, out var target))
            {
                Console.WriteLine($"[AddTransition] Target state '{to}' not found.");
                return null;
            }

            Trigger? trigger = null;
            string? guard = guardFromParser;

            // Determine if the first optional argument is a trigger or actually a guard
            if (!string.IsNullOrWhiteSpace(triggerNameOrGuard))
            {
                if (_triggers.TryGetValue(triggerNameOrGuard, out var foundTrigger))
                {
                    trigger = foundTrigger;
                }
                else
                {
                    // Assume it's a guard if it's not a known trigger
                    guard = triggerNameOrGuard;
                    Console.WriteLine($"[AddTransition] Interpreting '{triggerNameOrGuard}' as guard, not trigger.");
                }
            }

            Action? effect = null;
            if (_actions.TryGetValue(id, out var action))
            {
                effect = action;
            }

            var transition = new Transition(id, source, target, trigger, guard, effect);
            _transitions[id] = transition;

            Console.WriteLine($"[AddTransition] id='{id}', from='{from}', to='{to}', trigger='{trigger?.Id ?? "null"}', guard='{guard ?? "none"}', effect='{effect?.Id ?? "null"}'");
            return transition;
        }

        public Trigger? AddTrigger(string id, string description)
        {
            if (_triggers.ContainsKey(id))
            {
                Console.WriteLine($"[AddTrigger] Trigger with ID '{id}' already exists. Skipping.");
                return null;
            }

            var trigger = new Trigger
            {
                Id = id,
                Description = description
            };

            _triggers[id] = trigger;

            Console.WriteLine($"[AddTrigger] Added trigger: Id='{id}', Description='{description}'");
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
                        if (action.Id == state.Id) // or use a smarter link strategy if needed
                        {
                            simpleState.AddAction(action);
                            Console.WriteLine($"Connected ACTION '{action.Id}' to STATE '{state.Id}'");
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
