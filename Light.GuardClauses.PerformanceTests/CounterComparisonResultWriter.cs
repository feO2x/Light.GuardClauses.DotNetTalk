using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.GuardClauses.PerformanceTests
{
    public class CounterComparisonResultWriter
    {
        public virtual void WriteResults(string testHeader,
                                         IList<CounterPerformanceCandidate> orderedCandidates,
                                         IList<TimeSpan> performanceTestLengths)
        {
            Console.WriteLine(testHeader);
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(string.Empty);

            for (var i = 0; i < performanceTestLengths.Count; i++)
            {
                var length = performanceTestLengths[i];
                Console.WriteLine($"{length.TotalMilliseconds}ms test run:");

                foreach (var performanceCandidate in orderedCandidates)
                {
                    var testRunResult = performanceCandidate.TestRunResults[i];
                    Console.WriteLine($"{performanceCandidate.Name}: {testRunResult.NumberOfCalls:N0} executions (in {testRunResult.ElapsedTime.TotalMilliseconds:N0}ms)");
                }
                Console.WriteLine(string.Empty);
                Console.WriteLine(string.Empty);
            }
        }

        public virtual void WriteAverageBenchmarkStatistics(IList<CounterPerformanceCandidate> orderedCandidates)
        {
            var best = orderedCandidates.First();
            Console.WriteLine($"Average for {best.Name}: {best.GetAverageExecutionsPerMillisecond():N2} executions per ms");

            for (var i = 1; i < orderedCandidates.Count; i++)
            {
                var candidate = orderedCandidates[i];
                var ratioToBest = candidate.GetAverageExecutionsPerMillisecond() / best.GetAverageExecutionsPerMillisecond();
                Console.WriteLine($"Average for {candidate.Name}: {candidate.GetAverageExecutionsPerMillisecond():N2} executions per ms ({ratioToBest:P2} of best)");
            }
        }
    }
}