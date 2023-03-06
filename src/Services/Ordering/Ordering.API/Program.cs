using Core.Data;
using MassTransit;
using Ordering.API.EventBus.Consumers;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistance;
using System.Reflection;
/* MassTransit 
* ===========
* 1:Install the following package: 
*      1:masstransit
*      2:masstransit.rabbitmq
* 2:Add configuration section for  host in appsettings.cs
*  amqp://user:password@host:port
*  example: amqp://admin:admin@localhost:5672
* 3.1: As Producer 
* ================
* 
* 
* 4.1: As Consumer
* ================
*  create a class that implements interface IConsumer of type T where T is An Event Or Message
*  Ex: public class BascketCheckoutEventConsumer : IConsumer<BascketCheckoutEvent>
* 4.2:
*  implement Consume message for IConsumer interface
*  Ex:public Task Consume(ConsumeContext<BascketCheckoutEvent> context)
*  then you can use context.Message to get the received message
* 4.3: Inject & configure masstransit for rabbitMQ in startup 
    builder.Services.AddMassTransit(options =>
    {
        options.AddConsumer<BascketCheckoutEventConsumer>();
        options.UsingRabbitMq((ctx, config) =>
        {
            config.Host(builder.Configuration["RabbitMQ:Host"]);
            config.ConfigureEndpoints(ctx);
        });
    });
* 
*/
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IDbSettings dbSettings = builder.Configuration.GetSection(nameof(DbSettings)).Get<DbSettings>();

//clean code add DI for each project
builder.Services.AddApplicationServices();
builder.Services.AddInfrstructureServices(builder.Configuration,dbSettings.ConnectionString);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<BasketCheckoutEventConsumer>();

#region MassTransit Config

//Package 1: masstransit 2: masstransit.rabbitmq 
builder.Services.AddMassTransit(options =>
{
    options.AddConsumer<BasketCheckoutEventConsumer>();
    options.UsingRabbitMq((ctx, config) =>
    {
        config.Host(builder.Configuration["RabbitMQ:Host"]);
        config.ConfigureEndpoints(ctx);
    });
});
//Optional configuration
builder.Services.Configure<MassTransitHostOptions>(options =>
{
    options.WaitUntilStarted = true;
    options.StartTimeout = TimeSpan.FromSeconds(30);
    options.StopTimeout = TimeSpan.FromMinutes(1);
});

#endregion

var app = builder.Build();

app.AddMigration<OrderContext>((context, SP) => {
    var logger=app.Services.GetRequiredService<ILogger<OrderContextSeed>>();
    logger.LogInformation("Data Seeding.....");
    OrderContextSeed.SeedAsync(context, logger).Wait();
    //seed ddata
}, 0);



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
