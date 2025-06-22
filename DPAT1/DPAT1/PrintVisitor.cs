using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPAT1.Interfaces;

namespace DPAT1
{
    internal class PrintVisitor : IVisitor
    {
        public void Visit(ConsoleRenderer consoleRenderer)
        {
            var fsm = consoleRenderer.Fsm;

            Console.WriteLine("=== Finite State Machine ===\n");

            // Section: Actions
            var actions = GetPrivateActions(fsm);
            Console.WriteLine("Actions:");
            if (actions.Count == 0)
                Console.WriteLine("  (none)");
            foreach (var action in actions)
                Console.WriteLine($"  - {action.Id}");
            Console.WriteLine();

            // Section: Triggers
            var triggers = GetPrivateTriggers(fsm);
            Console.WriteLine("Triggers:");
            if (triggers.Count == 0)
                Console.WriteLine("  (none)");
            foreach (var trigger in triggers)
                Console.WriteLine($"  - {trigger.Id}");
            Console.WriteLine();

            // Section: States and transitions
            Console.WriteLine("States:");
            if (fsm.Children.Count == 0)
                Console.WriteLine("  (none)");

            foreach (var state in fsm.Children)
            {
                Console.WriteLine($"  • {state.Name} (Id: {state.Id}, Type: {state.Type})");

                var transitions = state.GetTransitions();
                if (transitions.Count == 0)
                {
                    Console.WriteLine("     └─ No outgoing transitions");
                }
                else
                {
                    Console.WriteLine("     └─ Outgoing transitions:");
                    foreach (var transition in transitions)
                    {
                        Console.WriteLine($"        --> {transition.Target.Id} on trigger '{transition.Trigger.Id}'");
                    }
                }
            }

            // Section: Global Transitions (if any)
            if (fsm.Transitions.Count > 0)
            {
                Console.WriteLine("\nGlobal Transitions:");
                foreach (var t in fsm.Transitions)
                {
                    Console.WriteLine($"  - {t.Source.Id} --> {t.Target.Id} on '{t.Trigger.Id}'");
                }
            }

            Console.WriteLine("\n=== End of FSM ===");
        }

        private List<Action> GetPrivateActions(FSM fsm)
        {
            var field = typeof(FSM).GetField("actions", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(fsm) as List<Action> ?? new();
        }

        private List<Trigger> GetPrivateTriggers(FSM fsm)
        {
            var field = typeof(FSM).GetField("triggers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(fsm) as List<Trigger> ?? new();
        }
    }
}
