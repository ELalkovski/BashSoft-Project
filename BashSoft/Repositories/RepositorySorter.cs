namespace BashSoft
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BashSoft.Contracts;

    public class RepositorySorter : IDataSorter
    {
        public void OrderAndTake(Dictionary<string, double> studentWithMarks, string comparison, int studentsToTake)
        {
            comparison = comparison.ToLower();

            if (comparison == "ascending")
            {
                this.PrintStudents(studentWithMarks.OrderBy(x => x.Value)
                    .Take(studentsToTake)
                    .ToDictionary(pair => pair.Key, pair => pair.Value));
            }
            else if (comparison == "descending")
            {
                this.PrintStudents(studentWithMarks.OrderByDescending(x => x.Value)
                    .Take(studentsToTake)
                    .ToDictionary(pair => pair.Key, pair => pair.Value));
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.InvalidComparisonQuery);
            }
        }

        private void PrintStudents(Dictionary<string, double> studentWithMarks)
        {
            foreach (var student in studentWithMarks)
            {
                OutputWriter.DisplayStudent(student);
            }
        }
    }
}
