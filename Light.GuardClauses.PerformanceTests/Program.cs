using System;
using Microsoft.Practices.Unity;

namespace Light.GuardClauses.PerformanceTests
{
    public class Program
    {
        private static void Main(string[] arguments)
        {
            arguments.Length.MustBe(1);

            var container = ConfigureContainer();

            if (arguments[0].ToLowerInvariant() == "all")
            {
                Console.WriteLine("Running performance tests...");
                var allTests = container.ResolveAll<IPerformanceTest>();

                foreach (var performanceTest in allTests)
                {
                    performanceTest.Run();
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Running performance test...");
                var singleTest = container.Resolve<IPerformanceTest>(arguments[0]);
                singleTest.Run();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press enter to quit...");
            Console.ReadLine();
        }

        private static IUnityContainer ConfigureContainer()
        {
            return new UnityContainer()
                .RegisterType<IPerformanceTest, MustNotBeNullMacrobenchmark>(nameof(MustNotBeNullMacrobenchmark))
                .RegisterType<IPerformanceTest, MustNotBeNullMicrobenchmark>(nameof(MustNotBeNullMicrobenchmark));
        }
    }
}