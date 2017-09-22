namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;

    [Alias("readDb")]
    public class ReadDatabaseCommand : Command
    {
        [Inject]
        private IDatabase repository;

        public ReadDatabaseCommand(string[] data, string input)
            : base(data, input)
        {  
        }

        public override void Execute()
        {
            if (this.Data.Length != 2)
            {
                throw new InvalidCommandException(this.Input);
            }

            var fileName = this.Data[1];
            this.repository.LoadData(fileName);
        }
    }
}
