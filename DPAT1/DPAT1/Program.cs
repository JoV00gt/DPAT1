using DPAT1;
using DPAT1.Interfaces;
using DPAT1.Strategies;
using DPAT1.View;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        string filePath = @"../../../FSMFiles/example_user_account.fsm";

        FSM? fsm = Build(filePath);
        if (fsm != null)
        {
            Validation(fsm);
            Render(fsm);
        }
    }
    private static FSM? Build(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return null;
        }

        FSMBuilder builder = new FSMBuilder();
        Director director = new Director(builder);
        director.BuildFSMFromFile(filePath, builder.GetFSM());
        return builder.GetFSM();
    }
    private static void Validation(FSM fsm)
    {
        var context = new FSMValidationContext();
        bool isNonDetValid = ValidateWith(context, fsm, new NonDeterministicTransitionsValidator(), "Non-deterministic transitions");
        bool isInitialValid = ValidateWith(context, fsm, new InitialStateTransitionsValidator(), "Initial state with incoming transitions");
        bool isReachableValid = ValidateWith(context, fsm, new UnreachableStateValidator(), "Unreachable states");

        bool allValid = isNonDetValid && isInitialValid && isReachableValid;
        Console.WriteLine();
        Console.WriteLine(allValid ? "FSM is Valid!" : "FSM contains validation errors");
    }

    private static void Render(FSM fsm)
    {
        IUIFactory factory = new ConsoleIOFactory(fsm);
        IOutputRenderer renderer = factory.CreateRenderer();
        IVisitor visitor = factory.CreateVisitor();
        renderer.Accept(visitor);
    }
    private static bool ValidateWith(FSMValidationContext context, FSM fsm, IFSMValidator strategy, string description)
    {
        context.SetStrategy(strategy);
        bool isValid = context.Validate(fsm);
        Console.WriteLine($"{description}: {(isValid ? "Valid" : "Invalid")}");
        return isValid;
    }
}