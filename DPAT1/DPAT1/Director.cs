using DPAT1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            var transitionList = new List<Transition>();

            string[] lines = File.ReadAllLines(file);

            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("TRIGGER"))
                {
                    var trigger = ParseTriggerDefinition(line);
                    if (trigger != null)
                    {
                        fsm.AddTrigger(trigger);
                        triggerDict[trigger.Id] = trigger;
                        Console.WriteLine($"Parsed TRIGGER: {trigger.Id}");
                    }
                }
                else if (line.StartsWith("STATE"))
                {
                    var state = ParseStateDefinition(line);
                    if (state != null)
                    {
                        fsm.AddState(state);
                        stateDict[state.Id] = state;
                        Console.WriteLine($"Parsed STATE: {state.Id}");
                    }
                }
                else if (line.StartsWith("ACTION"))
                {
                    var action = ParseActionDefinition(line);
                    if (action != null)
                    {
                        fsm.AddAction(action);
                        actionDict[action.Id] = action;
                        Console.WriteLine($"Parsed ACTION: {action.Id} ({action.Type})");
                    }
                }
                else if (line.StartsWith("TRANSITION"))
                {
                    var transition = ParseTransitionDefinition(line);
                    if (transition != null)
                    {
                        fsm.AddTransition(transition);
                        transition.Source.AddTransition(transition);
                        transitionList.Add(transition);
                        Console.WriteLine($"Parsed TRANSITION: {transition.Id}");
                    }
                }
            }

            builder.ConnectActionsToStates(stateDict, actionDict);
            return fsm;
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
            var match = Regex.Match(trimmedLine, @"^TRANSITION\s+(\S+)\s+(\S+)\s*->\s*(\S+)(?:\s+(\S+))?\s*(?:""([^""]*)"")?\s*;?\s*$");

            if (match.Success)
            {
                string id = match.Groups[1].Value;
                string from = match.Groups[2].Value;
                string to = match.Groups[3].Value;
                string trigger = null;
                string guard = null;

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
                string stateType = match.Groups[4].Value;

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
