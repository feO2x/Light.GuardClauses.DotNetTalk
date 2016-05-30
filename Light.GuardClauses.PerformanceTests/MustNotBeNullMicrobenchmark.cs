﻿using System;
using FluentAssertions;

namespace Light.GuardClauses.PerformanceTests
{
    public sealed class MustNotBeNullMicrobenchmark : BaseCounterComparisonTest, IPerformanceTest
    {
        public void Run()
        {
            var candidates = new[]
                             {
                                 new CounterPerformanceCandidate("Imperative Null Check", () => CheckForNullImperatively(new object())),
                                 new CounterPerformanceCandidate("Light.GuardClauses", () => CheckForNullWithLightGuardClauses(new object())),
                                 new CounterPerformanceCandidate("FluentAssertions", () => CheckForNullWithFluentAssertions(new object()))
                             };

            RunPerformanceTest("MustNotBeNull Microbenchmark", candidates);
        }

        private CounterTestRunResult CheckForNullImperatively(object @object)
        {
            var numberOfLoopRuns = 0UL;

            Stopwatch.Start();
            while (Continue)
            {
                if (@object == null)
                    throw new ArgumentNullException(nameof(@object));

                numberOfLoopRuns++;
            }
            Stopwatch.Stop();

            return new CounterTestRunResult(numberOfLoopRuns, Stopwatch.Elapsed);
        }

        private CounterTestRunResult CheckForNullWithLightGuardClauses(object @object)
        {
            var numberOfLoopRuns = 0UL;

            Stopwatch.Start();
            while (Continue)
            {
                @object.MustNotBeNull(nameof(@object));

                numberOfLoopRuns++;
            }
            Stopwatch.Stop();

            return new CounterTestRunResult(numberOfLoopRuns, Stopwatch.Elapsed);
        }

        private CounterTestRunResult CheckForNullWithFluentAssertions(object @object)
        {
            var numberOfLoopRuns = 0UL;

            Stopwatch.Start();
            while (Continue)
            {
                @object.Should().NotBeNull();

                numberOfLoopRuns++;
            }
            Stopwatch.Stop();

            return new CounterTestRunResult(numberOfLoopRuns, Stopwatch.Elapsed);
        }
    }
}