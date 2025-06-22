using DPAT1;
using DPAT1.Context;
using DPAT1.Interfaces;
using DPAT1.Strategies;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        string filePath = @"../../../FSMFiles/example_user_account.fsm";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        FSMBuilder builder = new FSMBuilder();
        Director director = new Director(builder);
        director.CreateFSM(filePath, builder.GetFSM());
        var fsm = builder.GetFSM();
        ValidateFSM(fsm, validators);
        IUIFactory factory = new ConsoleIOFactory(fsm);
        IOutputRenderer renderer = factory.CreateRenderer();
        IVisitor visitor = factory.CreateVisitor();
        renderer.Accept(visitor);
    }

        var context = new FSMValidationContext();

        bool isNonDetValid = ValidateWith(context, fsm, new NonDeterministicTransitionsValidator(), "Non-deterministic transitions");
        bool isInitialValid = ValidateWith(context, fsm, new InitialStateTransitionsValidator(), "Initial state with incoming transitions");
        bool isReachableValid = ValidateWith(context, fsm, new UnreachableStateValidator(), "Unreachable states");

        bool allValid = isNonDetValid && isInitialValid && isReachableValid;
        Console.WriteLine();
        Console.WriteLine(allValid ? "FSM is Valid!" : "FSM contains validation errors");
    }

    private static bool ValidateWith(FSMValidationContext context, FSM fsm, IFSMValidator strategy, string description)
    {
        context.SetStrategy(strategy);
        bool isValid = context.Validate(fsm);
        Console.WriteLine($"{description}: {(isValid ? "Valid" : "Invalid")}");
        return isValid;
    }
}