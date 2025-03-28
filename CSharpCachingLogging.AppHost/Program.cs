var builder = DistributedApplication.CreateBuilder(args);

var cacheRedis = builder.AddRedis("cacheRedis", 5000)
                        .WithRedisInsight();


builder.AddProject<Projects.CSharpCaching_Redis_API>("redisAPI")
                      .WithReference(cacheRedis)
                      .WaitFor(cacheRedis);

builder.AddProject<Projects.CSharpCachingLogging_Log_API>("LoggingAPI");

builder.Build().Run();
