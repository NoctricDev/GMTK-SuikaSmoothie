using System;
using System.Runtime.Serialization;

namespace CSharpTools.Versioning
{
    public class VersionPatternException : Exception
    {
        public VersionPatternException()  { }

        public VersionPatternException(string message) : base(message)  { }

        public VersionPatternException(string message, Exception inner) : base(message, inner) { }
    }
}