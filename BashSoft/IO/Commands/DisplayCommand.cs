namespace BashSoft.IO.Commands
{
    using System;
    using System.Collections.Generic;
    using BashSoft.Attributes;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;

    [Alias("display")]
    public class DisplayCommand : Command
    {
        [Inject]
        private IDatabase repository;

        public DisplayCommand(string[] data, string input) 
            : base(data, input)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 3)
            {
                throw new InvalidCommandException(this.Input);
            }

            var entityToDisplay = Data[1];
            var sortType = this.Data[2];

            if (entityToDisplay.Equals("students", StringComparison.OrdinalIgnoreCase))
            {
                IComparer<IStudent> studentComparator = this.CreateStudentComparator(sortType);
                ISimpleOrderedBag<IStudent> list = this.repository.GetAllStudentsSorted(studentComparator);
                OutputWriter.WriteMessageOnNewLine(list.JoinWith(Environment.NewLine));
            }
            else if (entityToDisplay.Equals("courses", StringComparison.OrdinalIgnoreCase))
            {
                IComparer<ICourse> courseComparator = this.CreateCourseComparator(sortType);
                ISimpleOrderedBag<ICourse> list = this.repository.GetAllCoursesSorted(courseComparator);
                OutputWriter.WriteMessageOnNewLine(list.JoinWith(Environment.NewLine));
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }

        private IComparer<ICourse> CreateCourseComparator(string sortType)
        {
            if (sortType.Equals("ascending", StringComparison.OrdinalIgnoreCase))
            {
                return Comparer<ICourse>.Create(
                    (firstCourse, secondCourse) => firstCourse.CompareTo(secondCourse));
            }

            if (sortType.Equals("descending", StringComparison.OrdinalIgnoreCase))
            {
                return Comparer<ICourse>.Create(
                    (firstCourse, secondCourse) => secondCourse.CompareTo(firstCourse));
            }

            throw new InvalidCommandException(this.Input);
        }

        private IComparer<IStudent> CreateStudentComparator(string sortType)
        {
            if (sortType.Equals("ascending", StringComparison.OrdinalIgnoreCase))
            {
                return Comparer<IStudent>.Create(
                    (firstStudent, secondStudent) => firstStudent.CompareTo(secondStudent));
            }

            if (sortType.Equals("descending", StringComparison.OrdinalIgnoreCase))
            {
                return Comparer<IStudent>.Create(
                    (firstStudent, secondStudent) => secondStudent.CompareTo(firstStudent));
            }

            throw new InvalidCommandException(this.Input);
        }
    }
}
