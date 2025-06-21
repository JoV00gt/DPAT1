
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
        FSM fsm = new FSM();
        string filePath = "C:\\Users\\wesle\\Documents\\Code\\DPAT1\\DPAT1\\DPAT1\\PrefabFiles\\example_lamp.fsm"; // Path to your FSM file

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }
        FSMBuilder builder = new FSMBuilder(); 
        Director director = new Director(builder);
        fsm = director.CreateFSM(filePath, fsm);

    }
}