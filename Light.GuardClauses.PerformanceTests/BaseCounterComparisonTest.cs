using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Light.GuardClauses.PerformanceTests
{
    public abstract class BaseCounterComparisonTest
    {
        private readonly Timer _timer;
        protected readonly Stopwatch Stopwatch = new Stopwatch();
        private CounterComparisonResultWriter _resultWriter = new CounterComparisonResultWriter();
        protected volatile bool Continue = true;
        protected List<TimeSpan> PerformanceTestLengths;

        protected BaseCounterComparisonTest(List<TimeSpan> performanceTestLenghts = null)
        {
            _timer = new Timer(StopPerformanceRun);

            PerformanceTestLengths = performanceTestLenghts ?? new List<TimeSpan>
                                                               {
                                                                   TimeSpan.FromMilliseconds(100),
                                                                   TimeSpan.FromMilliseconds(400),
                                                                   TimeSpan.FromMilliseconds(700),
                                                                   TimeSpan.FromMilliseconds(1000),
                                                                   TimeSpan.FromMilliseconds(2000),
                                                                   TimeSpan.FromMilliseconds(3000)
                                                               };
        }

        protected CounterComparisonResultWriter ResultWriter
        {
            get { return _resultWriter; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _resultWriter = value;
            }
        }

        private void StopPerformanceRun(object state)
        {
            Continue = false;
        }

        private void Reset()
        {
            Continue = true;
            Stopwatch.Reset();
        }

        private void StartTimer(TimeSpan interval)
        {
            _timer.Change(interval, TimeSpan.FromMilliseconds(-1));
        }

        protected void RunPerformanceTest(string testHeader, IList<CounterPerformanceCandidate> performanceCandidates)
        {
            foreach (var performanceTestLength in PerformanceTestLengths)
            {
                foreach (var performanceCandidate in performanceCandidates)
                {
                    StartTimer(performanceTestLength);
                    var testResult = performanceCandidate.RunTest();
                    performanceCandidate.TestRunResults.Add(testResult);

                    Reset();
                }
            }
            var orderedCandidates = performanceCandidates.OrderByDescending(c => c.GetAverageExecutionsPerMillisecond())
                                                         .ToList();

            ResultWriter.WriteResults(testHeader, orderedCandidates, PerformanceTestLengths);
            ResultWriter.WriteAverageBenchmarkStatistics(orderedCandidates);
        }
    }
}