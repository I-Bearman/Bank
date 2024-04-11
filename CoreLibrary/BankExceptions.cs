using System;

namespace CoreLibrary
{
    public class UncorrectSumWriteException : Exception
    {
        public UncorrectSumWriteException(string Msg) : base(Msg) { }
    }
    public class DontEnoughSumException : Exception
    {
        public DontEnoughSumException(string Msg) : base(Msg) { }
    }
}
