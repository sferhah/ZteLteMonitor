using Android.OS;
using ZteLteMonitor.Core;

namespace ZteLteMonitor.Droid
{
    public static class BundleExtenstions
    {
        public static readonly string JobActionKey = "job_action";
        public static PersistableBundle AsPersistantBundle(this MainJobOptions jobOptions)
        {
            var extras = new PersistableBundle();
            extras.PutString(nameof(MainJobOptions.IpAddress), jobOptions.IpAddress);
            extras.PutString(nameof(MainJobOptions.Password), jobOptions.Password);
            extras.PutBoolean(nameof(MainJobOptions.UseDefaultGateway), jobOptions.UseDefaultGateway);
            return extras;
        }

        public static Bundle AsBundle(this MainJobOptions jobOptions)
        {
            var extras = new Bundle();
            extras.PutString(nameof(MainJobOptions.IpAddress), jobOptions.IpAddress);
            extras.PutString(nameof(MainJobOptions.Password), jobOptions.Password);
            extras.PutBoolean(nameof(MainJobOptions.UseDefaultGateway), jobOptions.UseDefaultGateway);
            return extras;
        }

        public static MainJobOptions ToMainJobOptions(this Bundle extras) => new MainJobOptions
        {
            IpAddress = extras?.GetString(nameof(MainJobOptions.IpAddress), null) ?? null,
            Password = extras?.GetString(nameof(MainJobOptions.Password), null) ?? null,
            UseDefaultGateway = extras?.GetBoolean(nameof(MainJobOptions.UseDefaultGateway), false) ?? false,
        };

        public static MainJobOptions ToMainJobOptions(this PersistableBundle extras) => new MainJobOptions
        {
            IpAddress = extras?.GetString(nameof(MainJobOptions.IpAddress), null) ?? null,
            Password = extras?.GetString(nameof(MainJobOptions.Password), null) ?? null,
            UseDefaultGateway = extras?.GetBoolean(nameof(MainJobOptions.UseDefaultGateway), false) ?? false,
        };


        public static Bundle AsBundle(this AppLog jobResult)
        {
            var extras = new Bundle();
            extras.PutInt(nameof(AppLog.Id), jobResult.Id);
            extras.PutLong(nameof(AppLog.StartDate), jobResult.StartDate.Ticks);
            if (jobResult.EndDate != null)
            {
                extras.PutLong(nameof(AppLog.EndDate), jobResult.EndDate.Value.Ticks);
            }
            extras.PutString(nameof(AppLog.IpAddress), jobResult.IpAddress);
            extras.PutString(nameof(AppLog.Password), jobResult.Password);
            extras.PutInt(nameof(AppLog.Status), (int)jobResult.Status);
            extras.PutInt(nameof(AppLog.RebootStatus), (int)jobResult.RebootStatus);
            extras.PutString(nameof(AppLog.Error), jobResult.Error);
            extras.PutInt(nameof(AppLog.State), (int)jobResult.State);

            return extras;
        }

        public static AppLog ToAppLog(this Bundle extras) => new AppLog
        {
            Id = extras?.GetInt(nameof(AppLog.Id), -1) ?? -1,
            StartDate = new System.DateTime(extras?.GetLong(nameof(AppLog.StartDate), 0) ?? 0),
            EndDate = new System.DateTime(extras?.GetLong(nameof(AppLog.EndDate), 0) ?? 0),
            IpAddress = extras?.GetString(nameof(AppLog.IpAddress), null) ?? null,
            Password = extras?.GetString(nameof(AppLog.Password), null) ?? null,
            Status = (ModemStatus)(extras?.GetInt(nameof(AppLog.Status), 0) ?? null),
            RebootStatus = (RebootStatus)(extras?.GetInt(nameof(AppLog.RebootStatus), 0) ?? null),
            Error = extras?.GetString(nameof(AppLog.Error), null) ?? null,
            State = (LogState)(extras?.GetInt(nameof(AppLog.State), 0) ?? 0),
        };
    }
}