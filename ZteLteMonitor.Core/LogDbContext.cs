using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ZteLteMonitor.Core
{
    public class LogDbContext : DbContext
    {
        internal static string? path;
        public LogDbContext() { }
        public DbSet<AppLog> Logs => Set<AppLog>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path = LogDbContext.path == null ? "logs.db3" : (LogDbContext.path + "/" + "logs.db3");
            optionsBuilder.UseSqlite($"Filename={path}");
        }
    }

    public static class LogDbContextExt
    {
        public static async Task<AppLog?> Init(this LogDbContext ctx, string? path, int maxHours)
        {
            LogDbContext.path = path;
            AppLog? lastSuccessfulReboot = null;

            if (!await ctx.Database.EnsureCreatedAsync())
            {
                try
                {
                    lastSuccessfulReboot = await ctx.Logs
                        .FirstOrDefaultAsync(x => x.Status == ModemStatus.Disconnected
                        && x.RebootStatus == RebootStatus.Success);
                }
                catch // model has changed, recreate database
                {
                    await ctx.Database.EnsureDeletedAsync();
                    await ctx.Database.EnsureCreatedAsync();
                }
            }

            await ctx.DeleteLogsBefore(DateTime.UtcNow.AddHours(-maxHours));
            return lastSuccessfulReboot;
        }

        public static async Task DeleteLogsBefore(this LogDbContext ctx, DateTime dateTime)
        {
            var logs = await ctx.Logs.Where(x => x.StartDate < dateTime).ToListAsync();

            foreach (var log in logs)
            {
                ctx.Logs.Remove(log);
            }

            await ctx.SaveChangesAsync();
        }

        public static async Task EndLog(this LogDbContext ctx, AppLog log, bool cancelled = false)
        {
            log.EndDate = DateTime.UtcNow;
            log.State = cancelled ? LogState.Cancelled : LogState.Done;
            await ctx.SaveChangesAsync();
        }
    }
}