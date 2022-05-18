var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<PodcastDbContext>(options =>
        {
            options.UseMySql(
                hostContext.Configuration.GetConnectionString("PodcastDb"),
                new MySqlServerVersion(new Version(8, 0, 28))
            );
        });
        var feedQueueClient = new QueueClient(hostContext.Configuration.GetConnectionString("FeedQueue"), "feed-queue");
        feedQueueClient.CreateIfNotExists();
        services.AddSingleton(feedQueueClient);
        services.AddScoped<IPodcastIngestionHandler, PodcastIngestionHandler>();
        services.AddHttpClient<IFeedClient, FeedClient>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();