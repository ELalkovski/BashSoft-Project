namespace BashSoft
{
    using System;
    using System.Collections.Generic;
    using BashSoft.Contracts;

    public class RepositoryFilter : IDataFilter
    {
        public void FilterAndTake(Dictionary<string, double> studentWithMarks, string wantedFilter, int studentsToTake)
        {
            if (wantedFilter == "excellent")
            {
                this.FilterAndTake(studentWithMarks, x => x >= 5, studentsToTake);
            }
            else if (wantedFilter == "average")
            {
                this.FilterAndTake(studentWithMarks, x => x >= 3.5 && x < 5, studentsToTake);
            }
            else if (wantedFilter == "poor")
            {
                this.FilterAndTake(studentWithMarks, x => x < 3.5, studentsToTake);
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.InvalidStudentFilter);
            }
        }

        private void FilterAndTake(Dictionary<string, double> studentWithMarks, Predicate<double> givenFilter, int studentsToTake)
        {
            var counterForPrinted = 0;

            foreach (var studentMark in studentWithMarks)
            {
                if (counterForPrinted == studentsToTake)
                {
                    break;
                }

                if (givenFilter(studentMark.Value))
                {
                    OutputWriter.DisplayStudent(new KeyValuePair<string, double>(studentMark.Key, studentMark.Value));
                    counterForPrinted++;
                }
            }
        }
    }
}
