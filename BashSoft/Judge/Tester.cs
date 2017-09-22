namespace BashSoft
{
    using System;
    using System.IO;
    using BashSoft.Contracts;

    public class Tester : IContentComparer
    {
        public void CompareContent(string userOutputPath, string expectedOutputPath)
        {
            OutputWriter.WriteMessageOnNewLine("Reading files...");

            try
            {
                var mismatchPath = this.GetMismatchPath(expectedOutputPath);

                var actualOutputFiles = File.ReadAllLines(userOutputPath);
                var expectedOutputFiles = File.ReadAllLines(expectedOutputPath);

                bool hasMismatch;
                var mismatches = this.GetLinesWithPossibleMismatches(actualOutputFiles, expectedOutputFiles, out hasMismatch);

                this.PrintOutput(mismatches, hasMismatch, mismatchPath);
                OutputWriter.WriteMessageOnNewLine("Files read!");
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException(e.Message);
            }
        }

        private void PrintOutput(string[] mismatches, bool hasMismatch, string mismatchPath)
        {
            if (hasMismatch)
            {
                foreach (var mismatch in mismatches)
                {
                    OutputWriter.WriteMessageOnNewLine(mismatch);
                }

                File.WriteAllLines(mismatchPath, mismatches);  
            }
            else
            {
                OutputWriter.WriteMessageOnNewLine("Files are identical. There are no mismatches.");
            }
        }

        private string[] GetLinesWithPossibleMismatches(string[] actualOutputFiles, string[] expectedOutputFiles, out bool hasMismatch)
        {
            hasMismatch = false;
            string output = string.Empty;

            var mismatches = new string[actualOutputFiles.Length];
            OutputWriter.WriteMessageOnNewLine("Comparing files...");

            var minOutputLines = actualOutputFiles.Length;
            if (actualOutputFiles.Length != expectedOutputFiles.Length)
            {
                hasMismatch = true;
                minOutputLines = Math.Min(actualOutputFiles.Length, expectedOutputFiles.Length);
                OutputWriter.DisplayException(ExceptionMessages.ComparisonOfFilesWithDifferentSizes);
            }

            for (int i = 0; i < minOutputLines; i++)
            {
                var actualLine = actualOutputFiles[i];
                var expectedLine = expectedOutputFiles[i];

                if (!actualLine.Equals(expectedLine))
                {
                    output = string.Format("Mismatch line {0} -- expected: \"{1}\", actual: \"{2}\"", i, expectedLine, actualLine);
                    output += Environment.NewLine;
                    hasMismatch = true;
                }
                else
                {
                    output = actualLine;
                    output += Environment.NewLine;
                }

                mismatches[i] = output;
            }

            return mismatches;
        }

        private string GetMismatchPath(string expectedOutputPath)
        {
            var indexOf = expectedOutputPath.LastIndexOf("\\");
            var directoryPath = expectedOutputPath.Substring(0, indexOf);
            var finalPath = directoryPath + @"\Mismatches.txt";
            return finalPath;
        }
    }
}