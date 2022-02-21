using System;
using System.Linq;

namespace CLangAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Enter c source code filename to analyze: ");
                var cFileName = Console.ReadLine();
                try
                {
                    var funcCallsAggregator = new CLangFuncCallAggregator();
                    using (var analyzer = new CLangAnalyzer())
                    {
                        analyzer.Dispose();
                        analyzer.ParseFile(cFileName);
                        analyzer.Visitors.Add(funcCallsAggregator);
                        analyzer.Analyze();
                    }

                    var funcCalls = funcCallsAggregator.GetFuncCalls();
                    var recursiveFuncFinder = new RecursiveFuncFinder(funcCalls);
                    var recursiveFuncs = recursiveFuncFinder.FindPotentialRecursiveFuncs();

                    Console.WriteLine("Detected funcs: " + string.Join(", ", funcCalls.Select(fc => fc.Key)));
                    Console.WriteLine("Potential recursive funcs: " + string.Join(", ", recursiveFuncs));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while source file processing: " + ex);
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
