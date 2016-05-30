using System.Collections.Generic;
using System.Diagnostics;

namespace Light.GuardClauses.PerformanceTests
{
    public abstract class BaseComparisonBenchmark
    {
        protected readonly Stopwatch Stopwatch = new Stopwatch();

        private readonly ComparisonBenchmarkResultWriter _resultWriter = new ComparisonBenchmarkResultWriter();


        public void RunPerformanceTests(string testHeader, List<ComparisonCandidate> performanceCandidates, List<List<int>> testValues)
        {
            foreach (var passedValue in testValues)
            {
                foreach (var candidate in performanceCandidates)
                {
                    var duration = candidate.RunTest(passedValue);
                    candidate.Results.Add(duration);

                    Stopwatch.Reset();
                }
            }

            _resultWriter.WriteResults(testHeader, performanceCandidates, testValues);
        }
    }
}