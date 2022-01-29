using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Util;
using System.Threading;
using System.Threading.Tasks;
using ZteLteMonitor.Core;

namespace ZteLteMonitor.Droid
{
    [Service(Name = "ZteLteMonitor.Droid.MainJob", Permission = "android.permission.BIND_JOB_SERVICE")]
    public class MainJob : JobService
    {
        public static readonly string TAG = typeof(MainJob).FullName;

        private CancellationTokenSource _cancellation;
        public override bool OnStartJob(JobParameters jobParams)
        {
            _cancellation = new CancellationTokenSource();
            var options = jobParams.Extras.ToMainJobOptions();

            if (options.UseDefaultGateway)
            {
                options.IpAddress = NetworkUtils.GetDefaultGateway(this) ?? options.IpAddress;
            }

            _ = Task.Run(async () =>
            {
                await new MainService().MainMethod(new MainServiceArgs
                {
                    SqliteDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                    OnCallback = BroadcastResults,
                    Repeat = true,
                    Options = new MainServiceOptions
                    {
                        IpAddress = options.IpAddress,
                        Password = options.Password,
                    }
                },
                _cancellation.Token);

                JobFinished(jobParams, true);//  true: reschedule the job.
            });


            return true; // true: it runs on another thread
        }

        public override bool OnStopJob(JobParameters @params)
        {
            Log.Debug(TAG, "System halted the job.");

            _cancellation?.Cancel();

            BroadcastResults(new AppLog());

            return true; //  true: reschedule the job.
        }


        public void BroadcastResults(AppLog result)
        {
            Intent i = new Intent(BundleExtenstions.JobActionKey);
            i.PutExtras(result.AsBundle());
            BaseContext.SendBroadcast(i);
        }
    }
}