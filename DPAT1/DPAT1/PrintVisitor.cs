using System;
using System.Collections.Generic;
using DPAT1.Interfaces;

namespace DPAT1
{
    internal class PrintVisitor : IVisitor
    {
        public void Visit(ConsoleRenderer consoleRenderer)
        {
            var fsm = consoleRenderer.Fsm;

            Console.WriteLine("=== Finite State Machine ===\n");

            PrintCollection("Actions", fsm.GetActions(), a => $"- {a.Id}");

            PrintCollection("Triggers", fsm.GetTriggers(), t => $"- {t.Id}");

            Console.WriteLine("States:");
            if (fsm.Children.Count == 0)
            {
                Console.WriteLine("  (none)");
            }
            else
            {
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
                            string triggerId = transition.Trigger?.Id ?? "guard";
                            string guardText = !string.IsNullOrWhiteSpace(transition.Guard)
                                ? $" [guard: {transition.Guard}]"
                                : "";
                            Console.WriteLine($"        --> {transition.Target.Id} on trigger '{triggerId}'{guardText}");
                        }
                    }
                }
            }

            if (fsm.Transitions.Count > 0)
            {
                Console.WriteLine("\nGlobal Transitions:");
                foreach (var t in fsm.Transitions)
                {
                    string triggerText = t.Trigger != null ? $"'{t.Trigger.Id}'" : "guard";
                    string guardText = !string.IsNullOrWhiteSpace(t.Guard) ? $" [guard: {t.Guard}]" : "";
                    Console.WriteLine($"  - {t.Source.Id} --> {t.Target.Id} on {triggerText}{guardText}");
                }
            }

            Console.WriteLine("\n=== End of FSM ===");
        }

        private void PrintCollection<T>(string title, List<T> items, Func<T, string> formatter)
        {
            Console.WriteLine($"{title}:");
            if (items.Count == 0)
            {
                Console.WriteLine("  (none)");
            }
            else
            {
                foreach (var item in items)
                {
                    Console.WriteLine($"  {formatter(item)}");
                }
            }
            Console.WriteLine();
        }
    }
}