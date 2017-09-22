namespace BashSoft
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using BashSoft.Contracts;
    using BashSoft.DataStructures;
    using BashSoft.Models;

    public class StudentRepository : IDatabase
    {
        private bool isDataInitialized = false;
        private Dictionary<string, ICourse> courses;
        private Dictionary<string, IStudent> students;
        private IDataFilter filter;
        private IDataSorter sorter;

        public StudentRepository(IDataFilter filter, IDataSorter sorter)
        {
            this.filter = filter;
            this.sorter = sorter;
        }

        public void LoadData(string fileName)
        {
            if (this.isDataInitialized)
            {
                OutputWriter.DisplayException(ExceptionMessages.DataAlreadyInitializedException);
                return;
            }

            OutputWriter.WriteMessageOnNewLine("Reading data...");
            this.students = new Dictionary<string, IStudent>();
            this.courses = new Dictionary<string, ICourse>();
            this.ReadData(fileName);
        }

        public void UnloadData()
        {
            if (!this.isDataInitialized)
            {
                throw new ArgumentException(ExceptionMessages.DataAlreadyInitializedException);
            }

            this.students = null;
            this.courses = null;
            this.isDataInitialized = false;
        }

        public void GetStudentScoresFromCourse(string courseName, string studentName)
        {
            if (this.IsQueryForStudentPossible(courseName, studentName))
            {
                OutputWriter.DisplayStudent(new KeyValuePair<string, double>(studentName, this.courses[courseName].StudentsByName[studentName]
                    .MarksByCourseName[courseName]));
            }
        }

        public void GetAllStudentsFromCourse(string courseName)
        {
            if (this.IsQueryForCoursePossible(courseName))
            {
                OutputWriter.WriteMessageOnNewLine(courseName);

                foreach (var studentMarkEntry in this.courses[courseName].StudentsByName)
                {
                    this.GetStudentScoresFromCourse(courseName, studentMarkEntry.Key);
                }
            }
        }

        public ISimpleOrderedBag<ICourse> GetAllCoursesSorted(IComparer<ICourse> comparer)
        {
            ISimpleOrderedBag<ICourse> sortedCourses = new SimpleSortedList<ICourse>(comparer);
            sortedCourses.AddAll(this.courses.Values);

            return sortedCourses;
        }

        public ISimpleOrderedBag<IStudent> GetAllStudentsSorted(IComparer<IStudent> comparer)
        {
            ISimpleOrderedBag<IStudent> sortedStudents = new SimpleSortedList<IStudent>(comparer);
            sortedStudents.AddAll(this.students.Values);

            return sortedStudents;
        }

        public void FilterAndTake(string courseName, string givenFilter, int? studentsToTake = null)
        {
            if (this.IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = this.courses[courseName].StudentsByName.Count;
                }

                Dictionary<string, double> marks = this.courses[courseName].StudentsByName
                    .ToDictionary(x => x.Key, x => x.Value.MarksByCourseName[courseName]);

                this.filter.FilterAndTake(marks, givenFilter, studentsToTake.Value);
            }
        }

        public void OrderAndTake(string courseName, string comparison, int? studentsToTake = null)
        {
            if (this.IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = this.courses[courseName].StudentsByName.Count;
                }

                Dictionary<string, double> marks =
                    this.courses[courseName].StudentsByName
                        .ToDictionary(x => x.Key, x => x.Value.MarksByCourseName[courseName]);

                this.sorter.OrderAndTake(marks, comparison, studentsToTake.Value);
            }
        }

        private void ReadData(string fileName)
        {
            var path = SessionData.CurrentPath + "\\" + fileName;
            if (File.Exists(path))
            {
                var pattern = @"([A-Z][a-zA-Z#\++]*_[A-Z][a-z]{2}_\d{4})\s+([A-Za-z]+\d{2}_\d{2,4})\s([\s0-9]+)";
                var rgx = new Regex(pattern);
                var allInputLines = File.ReadAllLines(path);

                for (int line = 0; line < allInputLines.Length; line++)
                {
                    if (!string.IsNullOrEmpty(allInputLines[line]) && rgx.IsMatch(allInputLines[line]))
                    {
                        var currentMatch = rgx.Match(allInputLines[line]);

                        var courseName = currentMatch.Groups[1].Value;
                        var username = currentMatch.Groups[2].Value;
                        var scoresStr = currentMatch.Groups[3].Value;

                        try
                        {
                            var scores = scoresStr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray();

                            if (scores.Any(s => s > 100 || s < 0))
                            {
                                throw new ArgumentException(ExceptionMessages.InvalidScore);
                            }

                            if (scores.Length > SoftUniCourse.NumberOfTasksOnExam)
                            {
                                OutputWriter.DisplayException(ExceptionMessages.InvalidNumberOfScores);

                                continue;
                            }

                            if (!this.students.ContainsKey(username))
                            {
                                this.students.Add(username, new SoftUniStudent(username));
                            }

                            if (!this.courses.ContainsKey(courseName))
                            {
                                this.courses.Add(courseName, new SoftUniCourse(courseName));
                            }

                            ICourse course = this.courses[courseName];
                            IStudent student = this.students[username];

                            student.EnrollInCourses(course);
                            student.SetMarksOnCourse(courseName, scores);

                            course.EnrollStudent(student);
                        }
                        catch (Exception ex)
                        {
                            OutputWriter.DisplayException(ex.Message + $"at line: {line}");
                        }
                    }
                }
            }

            this.isDataInitialized = true;
            OutputWriter.WriteMessageOnNewLine("Data read!");
        }

        private bool IsQueryForCoursePossible(string courseName)
        {
            if (this.isDataInitialized)
            {
                if (this.courses.ContainsKey(courseName))
                {
                    return true;
                }
                else
                {
                    OutputWriter.DisplayException(ExceptionMessages.InexistingCourseInDataBase);
                }
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.DataNotInitializedExceptionMessage);
            }

            return false;
        }

        private bool IsQueryForStudentPossible(string courseName, string studentName)
        {
            if (this.IsQueryForCoursePossible(courseName) && this.courses[courseName].StudentsByName.ContainsKey(studentName))
            {
                return true;
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InexistingStudentInDataBase);
            }

            return false;
        }
    }
}