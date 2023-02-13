using Basket.API.Repositories;
using Basket.API.Services;
using Core.Caching;
using Core.Data;

/* run bash (cli for linux and unix = cmd in windows ) from docker container
 * docker exec -it container-name /bin/bash
 * Example => docker exec -it basketdb-server /bin/bash 
 * Run bash cli which resided in /bin directory
 * -it mean interactive terminal 
 * 
 * run redis-cli to open redis-cli terminal 
 * some of basic redis cli commands
 * 1: ping => check that server is a live
 * 2: set key value => set (add - edit ) value for key
 * 3: get key => get value for key
 * 4: del key => delete key
 * 5: flushall => remove all keys from all databases
 * 6: flushdb => remove all keys in a database 
 * 
 * 
 */

var builder = WebApplication.CreateBuilder(args);

#region Db AppSettings 

//map Appsettings Section To .net Class and Inject it
//
//1:options pattern
//accessing instance using IOption<DbSettings> => Constructor must be accept IOption<DbSettings>
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection(nameof(DbSettings)));

#endregion

// Add services to the container.

builder.Services.AddCache(options =>
{
    options.CacheType = CacheType.Redis;
    options.InstanceName = "Basket";
    options.ConnectionStr = $"{builder.Configuration["DbSettings:ServerName"]}:{builder.Configuration["DbSettings:PortNo"]}";
}
);


builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped(typeof(BasketService));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
