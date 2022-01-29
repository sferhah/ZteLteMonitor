using System;

namespace ZteLteMonitor.Core
{
    public class MainServiceOptions
    {
        public virtual string? IpAddress { get; set; }
        public virtual string? Password { get; set; }

        public virtual int IntervalInMilliseconds { get; set; } = 1000;        
        public virtual int DelayAfterRebootInMilliseconds { get; set; } = 45000;
        
        public virtual bool EnableSqliteLogs { get; set; } = true;
        public virtual int LoggingTimeInHours { get; set; } = 24;
    }

    public class MainServiceArgs
    {   
        public string? SqliteDirectory { get; set; }
        public Action<AppLog>? OnCallback { get; set; }
        public bool Repeat { get; set; }
        public MainServiceOptions Options { get; set; } = new MainServiceOptions();
    }
}