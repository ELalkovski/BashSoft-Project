namespace BashSoft.Contracts
{
    using System.Collections.Generic;

    public interface IRequester
    {
        void GetStudentScoresFromCourse(string courseName, string studentName);

        void GetAllStudentsFromCourse(string courseName);

        ISimpleOrderedBag<ICourse> GetAllCoursesSorted(IComparer<ICourse> comparer);

        ISimpleOrderedBag<IStudent> GetAllStudentsSorted(IComparer<IStudent> comparer);
    }
}
