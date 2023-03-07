using Ocelot.Middleware;
/*Ocelot gateway 
 * Free - Open Source API gateway for .net platform 
 * 
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

//builder.Con
var app = builder.Build();


app.UseOcelot();
app.MapGet("/", () => "Hello World!");

app.Run();
