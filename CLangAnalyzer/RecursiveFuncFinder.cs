using System.Collections.Generic;

namespace CLangAnalyzer
{
    public class RecursiveFuncFinder
    {
        public FuncCalls FuncCalls
        {
            get;
            set;
        }

        public RecursiveFuncFinder(FuncCalls funcCalls)
        {
            FuncCalls = funcCalls;
        }

        public string[] FindPotentialRecursiveFuncs()
        {
            var result = new List<string>(FuncCalls.Count);

            // mark visited functions to prevent endless recursion.
            var markedFunction = new Dictionary<string, bool>(FuncCalls.Count);
            foreach (var funcCall in FuncCalls)
            {
                ClearMarkedFunctions(markedFunction);

                var recursiveCallFound = FindRecursiveCall(markedFunction, funcCall.Key, funcCall.Key);
                if (recursiveCallFound)
                    result.Add(funcCall.Key);
            }

            result.Sort();
            return result.ToArray();
        }

        private void ClearMarkedFunctions(Dictionary<string, bool> markedFuncs)
        {
            foreach (var fc in FuncCalls)
                markedFuncs[fc.Key] = false;
        }

        private bool FindRecursiveCall(Dictionary<string, bool> markedFuncs, string funcName, string startFuncName)
        {
            foreach (var funcCall in FuncCalls[funcName])
            {
                if (!markedFuncs[funcCall])
                {
                    markedFuncs[funcCall] = true;
                    if (funcCall == startFuncName)
                        return true;

                    if (FindRecursiveCall(markedFuncs, funcCall, startFuncName))
                        return true;
                }
            }
            return false;
        }
    }
}
