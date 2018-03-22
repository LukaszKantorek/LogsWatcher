using System;

namespace LogsWatcher.Model
{
    [Flags]
    public enum LogTypeEnum
    {
        Trace = 0,
        Warning = 1,
        Error = 2,
        Fatal = 4
    }
}
