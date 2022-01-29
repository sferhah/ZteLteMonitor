using Android.App.Job;
using Android.Content;
using Java.Lang;

namespace ZteLteMonitor.Droid
{
    public class JobConstraints
    {
        public bool Persisted { get; set; } = true; // android.permission.RECEIVE_BOOT_COMPLETED
        public long MinimumLatency { get; set; } = 1000; // Wait at least 1 second
        public long OverrideDeadline { get; set; } = 5000; // But no longer than 5 seconds
        public NetworkType RequiredNetworkType { get; set; } = NetworkType.Any;
    }

    public static class JobSchedulerHelpers
    {
        public static readonly int MainJobId = 110;

        public static bool IsJobServiceOn(this Context context)
            => context.GetPendingJob() != null;

        public static void StopJobService(this Context context)
        {
            var job = context.GetPendingJob();

            if (job != null)
            {
                context.GetJobSchedulerService().Cancel(MainJobId);
            }
        }

        public static JobInfo GetPendingJob(this Context context)
            => context.GetJobSchedulerService().GetPendingJob(MainJobId);

        public static int CreateJob(this Context context, MainJobOptions options, JobConstraints jobConstraints = null)
            => context.GetJobSchedulerService()
                .Schedule(context.CreateJobBuilder()
                .SetExtras(options.AsPersistantBundle())
                .SetConstraints(jobConstraints)
                .Build());

        public static JobScheduler GetJobSchedulerService(this Context context)
            => (JobScheduler)context.GetSystemService(Context.JobSchedulerService);

        private static JobInfo.Builder CreateJobBuilder(this Context context)
            => new JobInfo.Builder(MainJobId, context.GetComponentNameForJob<MainJob>());

        private static ComponentName GetComponentNameForJob<T>(this Context context) where T : JobService
            => new ComponentName(context, Class.FromType(typeof(T)));

        private static JobInfo.Builder SetConstraints(this JobInfo.Builder builder, JobConstraints jobConstraints)
        {
            jobConstraints ??= new JobConstraints();

            builder.SetPersisted(jobConstraints.Persisted)
                .SetMinimumLatency(jobConstraints.MinimumLatency)
                .SetOverrideDeadline(jobConstraints.OverrideDeadline)
                .SetRequiredNetworkType(jobConstraints.RequiredNetworkType);
            //.SetPeriodic(jobConstraints.OverrideDeadline); // Can't call setOverrideDeadline() on a periodic job

            return builder;
        }
    }
}