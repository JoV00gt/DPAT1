
using DPAT1.Interfaces;

namespace DPAT1.View
{
    public class PrintVisitor : IVisitor
    {
        private const int ZERO = 0;
        public void Visit(ConsoleRenderer consoleRenderer)
        {
            FSM fsm = consoleRenderer.FSM;

            Console.WriteLine("=== Finite State Machine ===\n");

            PrintCollection("Actions", fsm.actions, a => $"- {a.Id}");

            PrintCollection("Triggers", fsm.triggers, t => $"- {t.Id}");

            Console.WriteLine("States:");
            StateRenderer(fsm);

            TransitionRenderer(fsm);

            Console.WriteLine("\n=== End of FSM ===");
        }

        private void StateRenderer(FSM fsm)
        {
            if (fsm.Children.Count == ZERO)
            {
                Console.WriteLine("  (none)");
            }
            else
            {
                foreach (var state in fsm.Children)
                {
                    Console.WriteLine($"  • {state.Name} (Id: {state.Id}, Type: {state.Type})");

                    var transitions = state.GetTransitions();
                    if (transitions.Count == ZERO)
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
        }

        private void TransitionRenderer(FSM fsm)
        {
            if (fsm.Transitions.Count > ZERO)
            {
                Console.WriteLine("\nGlobal Transitions:");
                foreach (var t in fsm.Transitions)
                {
                    string triggerText = t.Trigger != null ? $"'{t.Trigger.Id}'" : "guard";
                    string guardText = !string.IsNullOrWhiteSpace(t.Guard) ? $" [guard: {t.Guard}]" : "";
                    Console.WriteLine($"  - {t.Source.Id} --> {t.Target.Id} on {triggerText}{guardText}");
                }
            }
        }

        private void PrintCollection<T>(string title, List<T> items, Func<T, string> formatter)
        {
            Console.WriteLine($"{title}:");
            if (items.Count == ZERO)
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