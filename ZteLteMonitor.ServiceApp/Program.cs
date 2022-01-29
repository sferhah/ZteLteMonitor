using ZteLteMonitor.Core;
using ZteLteMonitor.ServiceApp;

using IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options => options.ServiceName = "Zte Lte Monitor")
    .UseSystemd()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton(new MainService());
        services.AddSingleton(hostContext.Configuration.GetSection("Options").Get<MainServiceOptions>());
        services.AddHostedService<MainBackgroundService>();
    })
    .Build();

await host.RunAsync();
