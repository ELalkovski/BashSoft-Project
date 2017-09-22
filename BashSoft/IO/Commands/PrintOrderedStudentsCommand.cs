namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;

    [Alias("order")]
    public class PrintOrderedStudentsCommand : Command
    {
        [Inject]
        private IDatabase repository;

        public PrintOrderedStudentsCommand(string[] data, string input)
            : base(data, input)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 5)
            {
                throw new InvalidCommandException(this.Input);
            }

            var courseName = this.Data[1];
            var filter = this.Data[2].ToLower();
            var takeCommand = this.Data[3].ToLower();
            var takeQuantity = this.Data[4].ToLower();

            this.TryParseParametersForOrderAndTake(takeCommand, takeQuantity, courseName, filter);
        }

        private void TryParseParametersForOrderAndTake(string takeCommand, string takeQuantity, string courseName, string filter)
        {
            if (takeCommand == "take")
            {
                if (takeQuantity == "all")
                {
                    this.repository.OrderAndTake(courseName, filter);
                }
                else
                {
                    int studentsToTake;
                    bool hasParsed = int.TryParse(takeQuantity, out studentsToTake);

                    if (hasParsed)
                    {
                        this.repository.OrderAndTake(courseName, filter, studentsToTake);
                    }
                    else
                    {
                        OutputWriter.DisplayException(ExceptionMessages.InvalidTakeQuantityParameter);
                    }
                }
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidTakeQuantityParameter);
            }
        }
    }
}
