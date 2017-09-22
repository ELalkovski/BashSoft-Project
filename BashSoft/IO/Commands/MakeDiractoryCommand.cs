namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;

    [Alias("mkdir")]
    public class MakeDiractoryCommand : Command
    {
        [Inject]
        private IDirectoryManager inputOutputManager;

        public MakeDiractoryCommand(string[] data, string input)
            : base(data, input)
        {   
        }

        public override void Execute()
        {
            if (this.Data.Length != 2)
            {
                throw new InvalidCommandException(this.Input);
            }

            var folderName = this.Data[1];
            this.inputOutputManager.CreateDirectoryInCurrentFolder(folderName);
        }
    }
}
