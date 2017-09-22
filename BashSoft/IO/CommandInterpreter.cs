namespace BashSoft.IO
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using BashSoft.Attributes;
    using BashSoft.Contracts;

    public class CommandInterpreter : IInterpreter
    {
        private IContentComparer judge;
        private IDatabase repository;
        private IDirectoryManager inputOutputManager;

        public CommandInterpreter(IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager)
        {
            this.judge = judge;
            this.repository = repository;
            this.inputOutputManager = inputOutputManager;
        }

        public void InterpredCommand(string input)
        {
            var data = input.Split(' ');
            var command = data[0];

            try
            {
                IExecutable currCommand = this.ParseCommand(data, command, input);
                currCommand.Execute();
            }
            catch (DirectoryNotFoundException dnfe)
            {
                OutputWriter.DisplayException(dnfe.Message);
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                OutputWriter.DisplayException(aoore.Message);
            }
            catch (ArgumentException ae)
            {
                OutputWriter.DisplayException(ae.Message);
            }
            catch (Exception e)
            {
                OutputWriter.DisplayException(e.Message);
            }
        }

        private IExecutable ParseCommand(string[] data, string command, string input)
        {
            object[] parametersForConstruction = new object[]
            {
                data,
                input
            };

            Type typeOfCommand = Assembly.GetExecutingAssembly()
                .GetTypes()
                .First(t => t.GetCustomAttributes(typeof(AliasAttribute))
                                .Where(at => at.Equals(command))
                                .ToArray().Length > 0);

            Type typeOfInterpreter = typeof(CommandInterpreter);

            IExecutable exe = (IExecutable) Activator.CreateInstance(typeOfCommand, parametersForConstruction);

            FieldInfo[] fieldsOfCommand = typeOfCommand.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo[] fieldsOfInterpreter = typeOfInterpreter.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (FieldInfo field in fieldsOfCommand)
            {
                Attribute atr = field.GetCustomAttribute(typeof(InjectAttribute));

                if (atr != null)
                {
                    if (fieldsOfInterpreter.Any(cf => cf.FieldType == field.FieldType))
                    {
                        field.SetValue(exe, fieldsOfInterpreter
                            .First(cf => cf.FieldType == field.FieldType)
                            .GetValue(this));
                    }
                }
            }

            return exe;
        }

        private void DisplayInvalidCommandMessage(string input)
        {
            OutputWriter.DisplayException($"The command '{input}' is invalid");
        }
    }
}
