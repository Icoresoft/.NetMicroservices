using Basket.API.Repositories;
using Basket.API.Services;
using Basket.API.Services.Grpc;
using Core.Caching;
using Core.Data;
using static Discount.Grpc.Protos.DiscountService;
using static Discount.Grpc.Protos.HealthCheckService;

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
 * Add Grpc Service As A Client
 * ================
 * 1- right click on your project => add => connected service
 * 2- Choose Grpc Service
 * 3- Browse to your .proto file , say XService
 * 4- Build your project, it will create a new class XServiceClient  
 * 4- create a .net class wrapper to wrap Grpc service functions in XServiceClient
 * 5- injecte the created wrapper class where you need to use it
 * 6- register (Inject) the Grpc service (e.g. XServiceClient ) in startup
 *    builder.Services.AddGrpcClient<XServiceClient>(options =>{ options.Address = });
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
    //todo : apply the same mechansim as discount service
    options.ConnectionStr = $"{builder.Configuration["DbSettings:ServerName"]}:{builder.Configuration["DbSettings:PortNo"]}";
}
);


builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped(typeof(BasketService));

//builder.Services.AddScoped(typeof(HealthCheckServiceClient));
builder.Services.AddGrpcClient<HealthCheckServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcService:Address"]);
});

builder.Services.AddGrpcClient<DiscountServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcService:Address"]);
});

builder.Services.AddScoped(typeof(GrpcHealthCheckService));
builder.Services.AddScoped(typeof(GrpcDiscountService));

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
