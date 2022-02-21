using NUnit.Framework;
using System.IO;
using System.Runtime.CompilerServices;

namespace CLangAnalyzer.UnitTests
{
    [TestFixture]
    public class RecursiveFuncFinderTests
    {
        private static readonly string SourceFilePath;

        static RecursiveFuncFinderTests()
        {
            SourceFilePath = Path.Combine(Path.GetDirectoryName(InitSamplesDirectory()) ?? string.Empty, "Source");
        }

        static string InitSamplesDirectory([CallerFilePath] string sourceFilePath = "")
        {
            return sourceFilePath;
        }

        [Test]
        public void PtSample()
        {
            var funcs = AnalyzeFileAndGetPotentialRecursiveFuncs(Path.Combine(SourceFilePath, "positive technologies sample.c"));
            CollectionAssert.AreEquivalent(new[] { "f", "g", "h" }, funcs);
        }

        [Test]
        public void EmptyFile()
        {
            var funcs = AnalyzeFileAndGetPotentialRecursiveFuncs(Path.Combine(SourceFilePath, "empty.c"));
            Assert.AreEqual(0, funcs.Length);
        }

        [Test]
        public void SubRecursive()
        {
            var funcs = AnalyzeFileAndGetPotentialRecursiveFuncs(Path.Combine(SourceFilePath, "sub recursive.c"));
            CollectionAssert.AreEquivalent(new[] { "g", "e" }, funcs);
        }

        [Test]
        public void IncorrectCode()
        {
            var fileName = Path.Combine(SourceFilePath, "incorrect code.c");
            Assert.Throws<CLangException>(() => AnalyzeFileAndGetPotentialRecursiveFuncs(fileName));
        }

        [Test]
        public void FunctionOverload()
        {
            // Fortunately c does not support function overload :)
            var fileName = Path.Combine(SourceFilePath, "function overload.c");
            Assert.Throws<CLangException>(() => AnalyzeFileAndGetPotentialRecursiveFuncs(fileName));
        }

        private string[] AnalyzeFileAndGetPotentialRecursiveFuncs(string fileName)
        {
            var funcCallsAggregator = new CLangFuncCallAggregator();
            using (var analyzer = new CLangAnalyzer())
            {
                analyzer.ParseFile(fileName);
                analyzer.Visitors.Add(funcCallsAggregator);
                analyzer.Analyze();
            }

            var funcCalls = funcCallsAggregator.GetFuncCalls();
            var recursiveFuncFinder = new RecursiveFuncFinder(funcCalls);
            var recursiveFuncs = recursiveFuncFinder.FindPotentialRecursiveFuncs();

            return recursiveFuncs;
        }
    }
}
