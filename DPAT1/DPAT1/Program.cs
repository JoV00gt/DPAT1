
using DPAT1;
using DPAT1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string filePath = "PrefabFiles/example_lamp.fsm"; // Path to your FSM file

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        var states = new List<string>();
        var triggers = new List<string>();
        var actions = new List<string>();
        var transitions = new List<string>();
        FSMBuilder builder = new FSMBuilder(); 
        Director director = new Director(builder);
        director.CreateFSM(filePath);
        foreach (var line in File.ReadLines(filePath))
        {
            string trimmedLine = line.Trim();

            // Skip comments and empty lines
            if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith("#"))
                continue;

            if (trimmedLine.StartsWith("STATE "))
                states.Add(trimmedLine);
            else if (trimmedLine.StartsWith("TRIGGER "))
                triggers.Add(trimmedLine);
            else if (trimmedLine.StartsWith("ACTION "))
                actions.Add(trimmedLine);
            else if (trimmedLine.StartsWith("TRANSITION "))
                transitions.Add(trimmedLine);
        }

        Console.WriteLine("== STATES ==");
        states.ForEach(Console.WriteLine);

        Console.WriteLine("\n== TRIGGERS ==");
        triggers.ForEach(Console.WriteLine);

        Console.WriteLine("\n== ACTIONS ==");
        actions.ForEach(Console.WriteLine);

        Console.WriteLine("\n== TRANSITIONS ==");
        transitions.ForEach(Console.WriteLine);
    }
}