using System;
using System.Collections.Generic;
using System.IO;

namespace loghash_testgen
{
    class TestGen
    {
        private const string TITLE_TAG = "#>";
        private const string COMMENT_TAG = "\\\\";

        public void GenerateFromFile(string filePath, string outputDirectory)
        {
            if (string.IsNullOrWhiteSpace(outputDirectory))
                throw new ArgumentNullException(nameof(outputDirectory));

            if (!File.Exists(filePath))
            {
                throw new ArgumentException("File does not exist.", nameof(outputDirectory));
                return;
            }

            var lines = File.ReadLines(filePath);

            string currentLine;
            IEnumerator<string> enumerator = lines.GetEnumerator();
            enumerator.MoveNext();

            TestCase currentTestCase = null;
            do
            {
                if (enumerator.Current.StartsWith(TITLE_TAG, StringComparison.CurrentCulture))
                {
                    if (currentTestCase != null)
                    {
                        WriteTestCase(outputDirectory, currentTestCase);
                        currentTestCase = null;
                    }

                    currentTestCase = new TestCase();
                    currentTestCase.Name = enumerator.Current.Substring(TITLE_TAG.Length);
                    enumerator.MoveNext();
                    currentTestCase.Content = enumerator.Current;
                    continue;
                }

                if (enumerator.Current.StartsWith(COMMENT_TAG) || string.IsNullOrWhiteSpace(enumerator.Current))
                    continue;

                currentTestCase.Json += enumerator.Current + Environment.NewLine;

            } while (enumerator.MoveNext());



            if (currentTestCase != null)
            {
                WriteTestCase(outputDirectory, currentTestCase);
                currentTestCase = null;
            }
        }

        private void WriteTestCase(string outputDirectory, TestCase currentTestCase)
        {
            EnsureDirectoryExists(outputDirectory);

            File.WriteAllText(Path.Combine(outputDirectory, currentTestCase.Name + ".txt"), currentTestCase.Content);
            File.WriteAllText(Path.Combine(outputDirectory, currentTestCase.Name + ".json"), currentTestCase.Json);
        }

        private void EnsureDirectoryExists(string outputDirectory)
        {
            if (Directory.Exists(outputDirectory))
                return;

            Directory.CreateDirectory(outputDirectory);
        }

        class TestCase
        {
            public string Name { get; set; }
            public string Content { get; set; }
            public string Json { get; set; }
        }
    }
}
