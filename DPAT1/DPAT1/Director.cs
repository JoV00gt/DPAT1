using DPAT1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void CreateFSM(IBuilder builder, string file) 
        {
            try
            {
                string[] lines = File.ReadAllLines(file);

                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();

                    if(string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#"))
                    {
                        continue;
                    }

                    if(trimmedLine.StartsWith("STATE"))
                    {
                        ParseStateDefinition(trimmedLine);
                    }
                    else if (trimmedLine.StartsWith("TRIGGER"))
                    {
                        ParseTriggerDefinition(trimmedLine);
                    }
                    else if (trimmedLine.StartsWith("ACTION"))
                    {
                        ParseActionDefinition(trimmedLine);
                    }
                    else if (trimmedLine.StartsWith("TRANSITION"))
                    {
                        ParseTransitionDefinition(trimmedLine);
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error reading FSM file: {ex.Message}");
                throw;
            }
        }

        private void ParseTriggerDefinition(string trimmedLine)
        {
            throw new NotImplementedException();
        }

        private void ParseTransitionDefinition(string trimmedLine)
        {
            throw new NotImplementedException();
        }

        private void ParseActionDefinition(string trimmedLine)
        {
            throw new NotImplementedException();
        }

        private void ParseStateDefinition(string trimmedLine)
        {
            throw new NotImplementedException();
        }
    }
}
