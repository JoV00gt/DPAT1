
using DPAT1;
using DPAT1.Interfaces;
using DPAT1.Strategies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class Program
{

    static void Main()
    {
        string filePath = @"../../../FSMFiles/example_lamp.fsm";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }
        FSMBuilder builder = new FSMBuilder(); 
        Director director = new Director(builder);
        director.CreateFSM(filePath, builder.GetFSM());

        var fsm = builder.GetFSM();
        ValidateFSM(fsm);

    }

    private static void ValidateFSM(FSM fsm)
    {
        var validators = new List<(IFSMValidator, string name)>
        {
            (new NonDeterministicTransitionsValidatorStrategy(), "Non-deterministic transitions"),
            (new InitialStateTransitionsValidatorStrategy(), "Initial state with incoming transitions"),
            (new UnreachableStateValidatorStrategy(), "Unreachable states")
        };

        bool allValid = true;

        foreach (var (validator, name) in validators)
        {
            if (validator.IsValid(fsm)) 
            {
                Console.WriteLine($"{name}: Valid");
            } else
            {
                Console.WriteLine($"{name}: Invalid");
                allValid = false;
            }
        }

        Console.WriteLine();
        if(allValid)
        {
            Console.WriteLine("FSM is Valid!");
        }
        else
        {
            Console.WriteLine("FSM contains validation errors");
        }
    }
}