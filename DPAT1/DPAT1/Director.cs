using DPAT1.Enums;
using DPAT1.Interfaces;

namespace DPAT1
{
    public class Director
    {
        private IBuilder builder;

        public Director(IBuilder builder)
        {
            this.builder = builder;
        }

        public FSM CreateFSM(string file, FSM fsm)
        {
            var stateDict = new Dictionary<string, IState>();
            var triggerDict = new Dictionary<string, Trigger>();
            var actionDict = new Dictionary<string, Action>();

            string[] lines = File.ReadAllLines(file);

            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                switch (GetDefinitionType(line))
                {
                    case DefinitionType.Trigger:
                        var trigger = ParseTriggerDefinition(line);
                        if (trigger != null)
                        {
                            fsm.AddTrigger(trigger);
                            triggerDict[trigger.Id] = trigger;
                        }
                        break;

                    case DefinitionType.State:
                        var state = ParseStateDefinition(line);
                        if (state != null)
                        {
                            fsm.AddState(state);
                            stateDict[state.Id] = state;
                        }
                        break;

                    case DefinitionType.Action:
                        var action = ParseActionDefinition(line);
                        if (action != null)
                        {
                            fsm.AddAction(action);
                            actionDict[action.Id] = action;
                        }
                        break;

                    case DefinitionType.Transition:
                        var transition = ParseTransitionDefinition(line);
                        if (transition != null)
                        {
                            fsm.AddTransition(transition);
                            transition.Source.AddTransition(transition);
                        }
                        break;

                    default:
                        break;
                }
            }

            builder.ConnectActionsToStates(stateDict, actionDict);
            return fsm;
        }

        private static DefinitionType GetDefinitionType(string line)
        {
            if (line.StartsWith("TRIGGER", StringComparison.OrdinalIgnoreCase)) return DefinitionType.Trigger;
            if (line.StartsWith("STATE", StringComparison.OrdinalIgnoreCase)) return DefinitionType.State;
            if (line.StartsWith("ACTION", StringComparison.OrdinalIgnoreCase)) return DefinitionType.Action;
            if (line.StartsWith("TRANSITION", StringComparison.OrdinalIgnoreCase)) return DefinitionType.Transition;
            return DefinitionType.Unknown;
        }

        private Trigger? ParseTriggerDefinition(string trimmedLine)
        {
            var match = RegexPatterns.Trigger.Match(trimmedLine);

            if (match.Success)
            {
                string name = match.Groups["name"].Value;
                string description = match.Groups["desc"].Value;
                return builder.AddTrigger(name, description);
            }
            return null;
        }

        private Transition? ParseTransitionDefinition(string trimmedLine)
        {
            var match = RegexPatterns.Transition.Match(trimmedLine);

            if (match.Success)
            {
                string id = match.Groups["id"].Value;
                string from = match.Groups["from"].Value;
                string to = match.Groups["to"].Value;
                string? trigger = match.Groups["trigger"].Success ? match.Groups["trigger"].Value : null;
                string? guard = match.Groups["guard"].Success ? match.Groups["guard"].Value : null;

                return builder.AddTransition(id, from, to, trigger, guard);
            }
            return null;
        }
        private Action? ParseActionDefinition(string trimmedLine)
        {
            var match = RegexPatterns.Action.Match(trimmedLine);

            if (match.Success)
            {
                string name = match.Groups["name"].Value;
                string description = match.Groups["desc"].Value;
                string actionType = match.Groups["type"].Value;

                return builder.AddAction(name, description, actionType);
            }
            return null;
        }

        private IState? ParseStateDefinition(string trimmedLine)
        {
            var match = RegexPatterns.State.Match(trimmedLine);

            if (match.Success)
            {
                string name = match.Groups["name"].Value;
                string parent = match.Groups["parent"].Value;
                string description = match.Groups["desc"].Value;
                string type = match.Groups["type"].Value;

                if (!Enum.TryParse<StateType>(type, true, out var stateType))
                {
                    return null;
                }

                return builder.AddState(name, parent, description, stateType);
            }
            return null;
        }

    }
}
