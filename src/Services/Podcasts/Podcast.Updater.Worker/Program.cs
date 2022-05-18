using Podcast.Updater.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services
            .AddDbContext<PodcastDbContext>(options =>
            {
                options.UseMySql(
                    hostContext.Configuration.GetConnectionString("PodcastDb"),
                    new MySqlServerVersion(new Version(8, 0, 28))
                );
            })
            .AddTransient<IPodcastUpdateHandler, PodcastUpdateHandler>()
            .AddHttpClient<IFeedClient, FeedClient>()
            .Services
            .AddLogging();
    })
    .Build();

await host.RunAsync();