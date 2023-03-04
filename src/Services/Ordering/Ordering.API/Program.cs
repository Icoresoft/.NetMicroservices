using Core.Data;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistance;

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
