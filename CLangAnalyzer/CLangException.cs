using System;

namespace CLangAnalyzer
{
    public class CLangException : Exception
    {
        public CLangException(string message)
            : base(message)
        {
        }
    }
}
