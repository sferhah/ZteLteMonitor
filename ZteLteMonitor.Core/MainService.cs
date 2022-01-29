using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZteLteMonitor.Core
{
    public class MainService
    {
        public async Task MainMethod(MainServiceArgs args, CancellationToken _cancellation = default)
        {

        BEGIN:
            try
            {
                await UnsafeMainMethod(args, args.Options, _cancellation);
            }
            catch (Exception ex)
            {
                args.OnCallback?.Invoke(new AppLog
                {
                    IpAddress = args.Options.IpAddress,
                    Password = args.Options.Password,
                    Error = ex.Message,
                });
            }

            if (args.Repeat && !_cancellation.IsCancellationRequested)
            {
                await Task.Delay(args.Options.IntervalInMilliseconds, _cancellation);
                goto BEGIN;
            }
        }

        public async Task UnsafeMainMethod(MainServiceArgs args, MainServiceOptions options, CancellationToken _cancellation = default)
        {
            #region sqlite
            using LogDbContext? ctx = options.EnableSqliteLogs ? new LogDbContext() : null;
            #endregion

            AppLog? lastSuccessfulReboot = null;

            #region sqlite
            if (ctx != null)
            {
                lastSuccessfulReboot = await ctx.Init(args.SqliteDirectory, options.LoggingTimeInHours);
            }
            #endregion

            var log = new AppLog
            {
                IpAddress = options.IpAddress,
                Password = options.Password,
            };

            #region sqlite
            if (ctx != null)
            {
                ctx.Logs.Add(log);
                await ctx.SaveChangesAsync();
            }
            #endregion
                    
            log.Status = IsRebootRequired(options, lastSuccessfulReboot?.StartDate) ? 
                ModemStatus.ScheduledReboot 
                : await ApiClient.GetRouterStatus(options.IpAddress);

            if (log.Status == ModemStatus.ScheduledReboot
                || log.Status == ModemStatus.Disconnected)
            {
                #region sqlite
                if (ctx != null)
                {
                    await ctx.SaveChangesAsync();
                }
                #endregion

                args.OnCallback?.Invoke(log);

                log.RebootStatus = await ApiClient.Reboot(options.IpAddress, options.Password);

                #region sqlite
                if (ctx != null)
                {
                    await ctx.SaveChangesAsync();
                }
                #endregion

                args.OnCallback?.Invoke(log);

                if (log.RebootStatus == RebootStatus.Success)
                {
                    await Task.Delay(options.DelayAfterRebootInMilliseconds, _cancellation);
                }
            }

            #region sqlite
            if (ctx != null)
            {
                await ctx.EndLog(log);
            }
            #endregion

            args.OnCallback?.Invoke(log);
        }


        public bool IsRebootRequired(MainServiceOptions options, DateTime? lastSuccessfulReboot)
        {
            //if(recurrenceTypeEnum includes today
            //&& recurrenceTime includes this hour)

            return false;
        }
        
    }
}