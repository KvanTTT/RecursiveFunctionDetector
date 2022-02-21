using ClangSharp;
using System;

namespace CLangAnalyzer
{
    public interface ICLangVisitor
    {
        CXChildVisitResult Visit(CXCursor cursor, CXCursor parent, IntPtr data);
    }
}
