using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
/*Ocelot gateway
 * =============
 * Free - Open Source API gateway for .net platform 
 * Packages:
 * 1: ocelot
 * 2: ocelot.cache.Cachemanager for caching purposes
 * 
 */
var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
    {
        logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
        logging.ClearProviders();
        logging.AddConsole();
        logging.AddDebug();
    });

    //.ConfigureAppConfiguration((ctx, config) =>
    //{
    //    config.AddJsonFile($"ocelot.{ctx.HostingEnvironment.EnvironmentName}.json", true, true);
    //});

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json").Build();
builder.Services.AddOcelot(configuration)
    .AddCacheManager(config=>config.WithDictionaryHandle()); // Enable Response Cache

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.UseOcelot().Wait();

app.MapGet("/", () => "Hello World!");
app.Run();
