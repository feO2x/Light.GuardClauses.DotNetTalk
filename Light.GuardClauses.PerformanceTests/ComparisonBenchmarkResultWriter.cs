using System;
using System.Collections.Generic;

namespace Light.GuardClauses.PerformanceTests
{
    public class ComparisonBenchmarkResultWriter
    {
        public virtual void WriteResults(string testHeader,
                                         IList<ComparisonCandidate> orderedCandidates,
                                         IList<List<int>> passedValues)
        {
            Console.WriteLine(testHeader);
            Console.WriteLine(new string('-', 40));
            Console.WriteLine();

            for (var i = 0; i < passedValues.Count; i++)
            {
                var passedValue = passedValues[i];
                Console.WriteLine(passedValue.ToString());

                foreach (var performanceCandidate in orderedCandidates)
                {
                    var testRunResult = performanceCandidate.Results[i];
                    Console.WriteLine($"{performanceCandidate.Name}: {testRunResult.TotalMilliseconds:N0}ms");
                }
                Console.WriteLine(string.Empty);
                Console.WriteLine(string.Empty);
            }
        }
    }
}