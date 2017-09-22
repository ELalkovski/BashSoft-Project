namespace BashSoft
{
    using System;
    using BashSoft.Contracts;

    public class InputReader : IReader
    {
        private const string EndCommand = "quit";

        private IInterpreter interpreter;

        public InputReader(IInterpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        public void StartReadingCommands()
        {
            while (true)
            {
                OutputWriter.WriteMessage($"{SessionData.CurrentPath}> ");
                var input = Console.ReadLine();
                input = input.Trim();
                if (input == EndCommand)
                {
                    break;
                }

                this.interpreter.InterpredCommand(input);
            }   
        }
    }
}