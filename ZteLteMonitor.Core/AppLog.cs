using System;
using System.Linq;

namespace ZteLteMonitor.Core
{
    public class AppLog
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }

        public ModemStatus Status { get; set; }
        public RebootStatus RebootStatus { get; set; }
        public string? IpAddress { get; set; }
        public string? Password { get; set; }
        public string? Error { get; set; }
        public LogState State { get; set; } = LogState.Pending;

        public override string ToString() => string.Join("\n", new string?[]
        {
            Id.ToString(),
            StartDate.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
            EndDate?.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
            IpAddress + ":" + Password,
            Status.ToString(),
            RebootStatus.ToString(),
            State.ToString(),
            Error,
        }.Where(x => x != null));

    }

    public enum LogState
    {
        Pending,
        Done,
        Cancelled,
    }

    public enum ModemStatus
    {
        None,
        PoweredOn,
        Disconnected,
        Connected,
        Unavailable,
        ScheduledReboot,
    }

    public enum RebootStatus
    {
        None,
        InvalidPassword,
        Success,        
        Failed,
        Unavailable,
    }
}