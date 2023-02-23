/*
 * GRPC Intro
 * ===========
 *  - it stands for  Google Remote Procedure Call
 *  - it depends on http2 
 *  - it 10x faster that rest API
 *  - it is used for synchronous communication espcially between microservices
 *  - Reliable & robust communication 
 *  
 *  How To Create A Grpc Service
 *  =============================
 * Create .Grpc Project 
 * there are two important folders in the created project
 * 1: protos  => which hold .proto files ( protocol buffer file )
 * 2: services => whcih contains classes that inhertes from generated classess from proto files
 * to start creating grpc service 
 * 1: add .proto file in protos file (say XService.proto)
 * 2: Right Click on XService.proto file then choose properties from menu
 * 3.1: choose Build action => Protobuf compiler  
 * 3.2: choose Stub Class => Server only to build grpc service as sever only
 * 4:Build your project, it wil generate a c# class from the created .proto file
 * 5:the generated class will be located in obj => Debug => net6.0 => Protos => XServiceBase
 * 6:Create  a service class ( XService ) in the services folder and inhert from the generated class ( XServiceBase)
 * 7:implement your rpc functions that defined in the proto file.
 * 8:register your service as grpc in the program.cs file 
 *  app.MapGrpcService<XService>();
 *  
 *  Add Automapper 
 *  ==============
 *  1:install automapper package
 *    install-package automapper.extensions.microsoft.dependencyinjecttion
 *  2:add automapper to the DI container in program.cs 
 *    builder.Services.AddAutoMapper(typeof(Program));
 *  3:add mapper folder in your project then
 *  4:add a mapper profile class  for your entities which inherts from Profile class
 *  5:create your map in the profile class contructor
 *    CreateMap<DestinationType,SourceType>().ReverseMap()
 *  6:inject automapper in your service class ( IMapper ) 
 *  7:use automapper e.g. _mapper.map<Destination>(Source);
 */
using Core.Data;
using Core.Extensions;
using Discount.Grpc.Repositories;
using Discount.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

//add automapper
builder.Services.AddAutoMapper(typeof(Program));

//Map and Inject AppSettings Section
//use this method to be able to access object of type DbSettings from ServiceProvider object
//1:
IDbSettings dbSettings = builder.Configuration.GetSection(nameof(DbSettings)).Get<DbSettings>();
builder.Services.AddSingleton(dbSettings);
//2:
builder.Services.AddNpgsqlConnection();
//3:
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
//4:
//builder.Services.AddScoped(typeof(CouponService));



var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGrpcService<HealthCheckService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

//extension method to Migrate Postgres database
app.MigratePGDatabase<Program>();

app.Run();
