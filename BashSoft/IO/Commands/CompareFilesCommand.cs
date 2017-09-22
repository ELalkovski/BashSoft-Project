namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;

    [Alias("cmp")]
    public class CompareFilesCommand : Command
    {
        [Inject]
        private IContentComparer judge;

        public CompareFilesCommand(string[] data, string input)
            : base(data, input)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 3)
            {
                throw new InvalidCommandException(this.Input);
            }

            var firstPath = this.Data[1];
            var secondPath = this.Data[2];

            this.judge.CompareContent(firstPath, secondPath);          
        }
    }
}