using ClangSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CLangAnalyzer
{
    public class CLangFuncCallAggregator : ICLangVisitor
    {
        private string _currentFuncName;
        private readonly Dictionary<string, HashSet<string>> _funcCalls;

        public CLangFuncCallAggregator()
        {
            _funcCalls = new Dictionary<string, HashSet<string>>();
        }

        public CXChildVisitResult Visit(CXCursor cursor, CXCursor parent, IntPtr data)
        {
            var curKind = clang.getCursorKind(cursor);

            if (curKind == CXCursorKind.CXCursor_FunctionDecl)
            {
                _currentFuncName = clang.getCursorSpelling(cursor).ToString();
                if (!_funcCalls.ContainsKey(_currentFuncName))
                    _funcCalls[_currentFuncName] = new HashSet<string>();
            }
            else if (curKind == CXCursorKind.CXCursor_CallExpr)
            {
                var callFuncName = clang.getCursorSpelling(cursor).ToString();
                _funcCalls[_currentFuncName].Add(callFuncName);
            }

            return CXChildVisitResult.CXChildVisit_Recurse;
        }

        public FuncCalls GetFuncCalls()
        {
            var result = new FuncCalls();
            foreach (var funcCall in _funcCalls)
                result[funcCall.Key] = funcCall.Value.ToArray();
            return result;
        }
    }
}
