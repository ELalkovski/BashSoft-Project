namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;

    [Alias("show")]
    public class ShowCourseCommand : Command
    {
        [Inject]
        private IDatabase repository;

        public ShowCourseCommand(string[] data, string input)
            : base(data, input)
        { 
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                var courseName = this.Data[1];
                this.repository.GetAllStudentsFromCourse(courseName);
            }
            else if (this.Data.Length == 3)
            {
                var courseName = this.Data[1];
                var username = this.Data[2];
                this.repository.GetStudentScoresFromCourse(courseName, username);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
