namespace BashSoft.IO.Commands
{
    using System;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;

    public abstract class Command : IExecutable
    {
        private string[] data;
        private string input;

        public Command(string[] data, string input)
        {
            this.Data = data;
            this.Input = input;
        }

        protected string[] Data
        {
            get
            {
                return this.data;
            }

            private set
            {
                if (value == null || value.Length == 0)
                {
                    throw new NullReferenceException();
                }

                this.data = value;
            }
        }

        protected string Input
        {
           get
           {
               return this.input;
           }

           private set
           {
               if (string.IsNullOrEmpty(value))
               {
                   throw new InvalidStringException();
               }

               this.input = value;
           }
        }

        public abstract void Execute();

        protected void InvalidCommandException()
        {
            OutputWriter.DisplayException($"The command '{this.input}' is invalid");
        }
    }
}