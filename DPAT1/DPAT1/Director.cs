using DPAT1.Enums;
using DPAT1.Interfaces;
using System.Text.RegularExpressions;

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
                            Console.WriteLine($"Parsed TRIGGER: {trigger.Id}");
                        }
                        break;

                    case DefinitionType.State:
                        var state = ParseStateDefinition(line);
                        if (state != null)
                        {
                            fsm.AddState(state);
                            stateDict[state.Id] = state;
                            Console.WriteLine($"Parsed STATE: {state.Id}");
                        }
                        break;

                    case DefinitionType.Action:
                        var action = ParseActionDefinition(line);
                        if (action != null)
                        {
                            fsm.AddAction(action);
                            actionDict[action.Id] = action;
                            Console.WriteLine($"Parsed ACTION: {action.Id} ({action.Type})");
                        }
                        break;

                    case DefinitionType.Transition:
                        var transition = ParseTransitionDefinition(line);
                        if (transition != null)
                        {
                            fsm.AddTransition(transition);
                            transition.Source.AddTransition(transition);
                            Console.WriteLine($"Parsed TRANSITION: {transition.Id}");
                        }
                        break;

                    default:
                        Console.WriteLine($"Unrecognized definition: {line}");
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
            // Format: TRIGGER <name> "<description>";
            var match = Regex.Match(trimmedLine, @"^TRIGGER\s+(\S+)\s+""([^""]+)"";?$");

            if (match.Success)
            {
                string name = match.Groups[1].Value;
                string description = match.Groups[2].Value;

                return builder.AddTrigger(name, description);
            }
            else
            {
                Console.WriteLine($"Could not parse trigger line: {trimmedLine}");
                return null;
            }
        }

        private Transition? ParseTransitionDefinition(string trimmedLine)
        {
            // Format: TRANSITION <id> <from_state> -> <to_state> [<trigger>] ["<guard>"];
            var match = Regex.Match(trimmedLine, @"^TRANSITION\s+(\S+)\s+(\S+)\s*->\s*(\S+)(?:\s+(\S+))?\s*(?:""([^""]*)"")?\s*;?\s*$");

            if (match.Success)
            {
                string id = match.Groups[1].Value;
                string from = match.Groups[2].Value;
                string to = match.Groups[3].Value;
                string? trigger = null;
                string? guard = null;

                if (match.Groups[5].Success)
                {
                    guard = match.Groups[5].Value;
                    if (match.Groups[4].Success)
                        trigger = match.Groups[4].Value;
                }
                else if (match.Groups[4].Success)
                {
                    trigger = match.Groups[4].Value;
                }

                return builder.AddTransition(id, from, to, trigger, guard);
            }
            else
            {
                Console.WriteLine($"Could not parse transition line: {trimmedLine}");
                return null;
            }
        }
        private Action? ParseActionDefinition(string trimmedLine)
        {
            // Format: ACTION <name> "<description>" : <action_type>;
            var match = Regex.Match(trimmedLine, @"^ACTION\s+(\S+)\s+""([^""]+)""\s*:\s*(\w+);?$");

            if (match.Success)
            {
                string name = match.Groups[1].Value;
                string description = match.Groups[2].Value;
                string actionType = match.Groups[3].Value;

                return builder.AddAction(name, description, actionType);
            }
            else
            {
                Console.WriteLine($"Could not parse action line: {trimmedLine}");
                return null;
            }
        }

        private IState? ParseStateDefinition(string trimmedLine)
        {
            // Format: STATE <name> <parent> "<description>" : <type>;
            var match = Regex.Match(trimmedLine, @"^STATE\s+(\S+)\s+(\S+)\s+""([^""]*)""\s*:\s*(\w+)\s*;?\s*$");

            if (match.Success)
            {
                string name = match.Groups[1].Value;
                string parent = match.Groups[2].Value;
                string description = match.Groups[3].Value;
                if (!Enum.TryParse<StateType>(match.Groups[4].Value, true, out var stateType))
                {
                    Console.WriteLine($"[ParseStateDefinition] Unknown state type: '{match.Groups[4].Value}'");
                    return null;
                }

                return builder.AddState(name, parent, description, stateType);
            }
            else
            {
                Console.WriteLine($"Could not parse state line: {trimmedLine}");
                return null;
            }
        }

    }
}
