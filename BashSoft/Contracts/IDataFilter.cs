namespace BashSoft.Contracts
{
    using System.Collections.Generic;

    public interface IDataFilter
    {
        void FilterAndTake(Dictionary<string, double> studentWithMarks, string wantedFilter, int studentsToTake);
    }
}
