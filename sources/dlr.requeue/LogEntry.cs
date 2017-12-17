using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Bespoke.PosEntt.DlqRequeue
{
    public class LogEntry
    {
        public LogEntry()
        {

        }

        public LogEntry([CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0)
        {

            this.CallerFilePath = filePath;
            this.CallerMemberName = memberName;
            this.CallerLineNumber = lineNumber;
            this.Computer = Environment.MachineName;
            this.User = Environment.UserName;
        }
        
        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Details))
                return this.Message;

            return $"{this.Message}\r\n{this.Details}";
        }

        public Exception Exception { get; }
        public Severity Severity { get; set; }
        public EventLog Log { get; set; }
        public string Source { get; set; }
        public string Operation { get; set; }
        public string User { get; set; }
        public string Computer { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public string Id { get; set; }
        public string[] Keywords { get; set; }
        public string Details { get; set; }
        public string CallerFilePath { get; set; }
        public string CallerMemberName { get; set; }
        public int CallerLineNumber { get; set; }
        public string Hash { get; set; }

        public Dictionary<string, object> OtherInfo { get; } = new Dictionary<string, object>();
    }

    public enum Severity
    {
        Debug = 0,
        Verbose = 1,
        Info = 2,
        Log = 3,
        Warning = 4,
        Error = 5,
        Critical = 6
    }


    public enum EventLog
    {
        Application,
        Security,
        Schedulers,
        Subscribers,
        WebServer,
        Elasticsearch,
        SqlRepository,
        SqlPersistence,
        Logger
    }
}