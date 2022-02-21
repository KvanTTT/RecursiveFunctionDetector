using ClangSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLangAnalyzer
{
    public class CLangAnalyzer : IDisposable
    {
        private CXIndex _index;
        private CXTranslationUnit _translationUnit;
        private bool _disposed;

        public List<ICLangVisitor> Visitors { get; }

        public CLangAnalyzer()
        {
            Visitors = new List<ICLangVisitor>();
        }

        ~CLangAnalyzer()
        {
            Dispose();
        }

        public void ParseFile(string cFile)
        {
            _index = clang.createIndex(0, 1);
            var args = new List<string>
            {
                "-std=c99",
                "-w"         // suppress warnings
            };

            var translationUnitError = clang.parseTranslationUnit2(_index, cFile, args.ToArray(), args.Count, out _, 0, 0, out _translationUnit);

            if (translationUnitError != CXErrorCode.CXError_Success)
                throw new CLangException(translationUnitError.ToString());

            var numDiagnostics = clang.getNumDiagnostics(_translationUnit);
            if (numDiagnostics > 0)
            {
                var errorString = new StringBuilder();
                errorString.AppendLine("Parse errors: ");
                for (uint i = 0; i < numDiagnostics; ++i)
                {
                    CXDiagnostic diagnostic = new CXDiagnostic();
                    try
                    {
                        diagnostic = clang.getDiagnostic(_translationUnit, i);
                        errorString.AppendLine(clang.getDiagnosticSpelling(diagnostic).ToString());
                    }
                    finally
                    {
                        clang.disposeDiagnostic(diagnostic);
                    }
                }
                throw new CLangException(errorString.ToString());
            }
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            clang.disposeTranslationUnit(_translationUnit);
            clang.disposeIndex(_index);
            _disposed = true;
        }

        public void Analyze()
        {
            foreach (var analyzer in Visitors)
            {
                var cursor = clang.getTranslationUnitCursor(_translationUnit);
                var clientData = new CXClientData(IntPtr.Zero);
                clang.visitChildren(cursor, analyzer.Visit, clientData);
            }
        }
    }
}
