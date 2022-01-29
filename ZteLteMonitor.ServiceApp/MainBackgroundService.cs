using ZteLteMonitor.Core;

namespace ZteLteMonitor.ServiceApp
{
    public sealed class MainBackgroundService : BackgroundService
    {
        private readonly MainService mainService;
        private readonly ILogger<MainBackgroundService> logger;
        private readonly MainServiceOptions options;

        public MainBackgroundService(MainService mainService, ILogger<MainBackgroundService> logger, MainServiceOptions options)
            => (this.mainService, this.logger, this.options) = (mainService, logger, options);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await mainService.MainMethod(new MainServiceArgs
            {
                SqliteDirectory = AppDomain.CurrentDomain.BaseDirectory, //if null path = C:\Windows\SysWOW64 || C:\Windows\System32
                OnCallback = log =>
                {
                    # if DEBUG
                    Console.WriteLine($"************************\n{log}\n________________________");
                    #endif

                    if (log.Status == ModemStatus.Disconnected
                    || !string.IsNullOrWhiteSpace(log.Error))
                    {
                        logger.LogWarning(log.ToString());
                    }
                },
                Repeat = true,
                Options = options
            }, stoppingToken);
        }
    }
}

