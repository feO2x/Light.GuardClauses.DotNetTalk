using System;

namespace Light.GuardClauses.PerformanceTests
{
    public struct CounterTestRunResult
    {
        public readonly ulong NumberOfCalls;
        public readonly TimeSpan ElapsedTime;

        public CounterTestRunResult(ulong numberOfCalls, TimeSpan elapsedTime)
        {
            NumberOfCalls = numberOfCalls;
            ElapsedTime = elapsedTime;
        }
    }
}