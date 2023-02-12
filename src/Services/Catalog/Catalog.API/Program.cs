
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using System.Runtime;
using Core.Data;
using System.Reflection;
using FluentValidation.AspNetCore;
using FluentValidation;
using Catalog.API.DbContext;
using Microsoft.Extensions.DependencyInjection;
using Catalog.API.Repositories;
using Catalog.API.Services;

#region Documentation
/**
* docker commands
* ===============
* 1:Run Docker Image
*  docker run -d -p host:container --name ContainerName ImageName
*      -d => deattached mode
*      -p => port Host Port: Container Port
*      --name => your container name 
*      ImageName => the image you would like to run it 
* Example=> docker run -d -p 27027:27027  --name Monogo-Shopping mongo
* 
* 2:List all running containers
*  docker ps
*  
* 3:List all containers either running or not 
*   docker container ls 
*   
* 4:list all images
*   docker image ls
*   
* 5:Run docker image if already existing
*   docker run image-id
* 
* 6:Start Container
*   docker start container-ID
*   
* 7:Stop Container
*   docker stop container-ID
*   
* 8:Remove container
*   docker rm container-ID
*   
* 9:remove image
*   docker rmi image-name
*   
* 10:Pull image from docker registery hub
*   docker pull image-name
*   
* 11:execute interactive terminal 
* docker exec it ContainerName  Path => it means interactive  terminal
* Example docker exec it Mongo-Shoping /bin/bash ==> bash is  command shell for linux and unix
* 
* 12:Rename Container 
*   docker rename CONTAINER NEW_NAME
* 
* 13:Docker compose ( run docker compose command where docker-compose.yml file existing 
*  docker-compose -p projectname -f yml-file-path up
*  -p  => Specify an alternate project name (default: directory name)
*  -up =>  Create and start containers
*  -f  => path-to-yaml-file  (default current running directory if -f param not passed)
*  Example => docker-compose -p catalogservice -f docker-compose.yml -f docker-compose.override.yml up
*  
*  14:stop and remove docker-compose containers
*   Instead of using up paramter use down parameter 
*   docker-compose -p projectname -f yml-file-path down
/**
 * Add Docker  compose
 * ====================
 * Compose is a tool for defining and running multi-container Docker applications. 
 * With Compose, you use a YAML file to configure your application’s services. 
 * Then, with a single command, you create and start all the services from your configuration.
 * 
 * 1- right click on your project => add => container orchestration suport
 * 2- it will add dockerfile to your project to build image for your project
 * 3- it will also create new project docker-compose and make it as a startup project
 * 4- docker-compose.yml file will initially contains your project image that 
 *    you use when adding container orchestraion support
 * 5- you can edit docker-compose.yml in docker-compose project to add more images 
 *    to the compose (images you need to run your application
 *    Example Catalog.API image  + MongoDb image
 * Notes:
 * ======
 * you can edit your application port number that docker expose it from 
 * dockerfile=>expose ContainerPortNo command 
 * you can add more environment to docker-compose by adding more files to docker-compose-yml
 * for example docker-compose.override.yml OR  docker-compose.dev.yml etc
 * 
 */
#endregion

var builder = WebApplication.CreateBuilder(args);


#region Fluent Validation 

//enable fluent validation for DTOs OR ViewModels
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


#endregion

#region Mongo Db Settings

//save GUID as string
//BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
//Save DateTime Object As String 
BsonSerializer.RegisterSerializer(new DateTimeSerializer(BsonType.String));

#endregion

#region Db AppSettings 

//map Appsettings Section To .net Class and Inject it
//
//1:options pattern
//accessing instance using IOption<DbSettings> => Constructor must be accept IOption<DbSettings>
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection(nameof(DbSettings)));
//2:Accessing object normally  as an injected object 
//IDbSettings dbSettings = builder.Configuration.GetSection(nameof(DbSettings)).Get<DbSettings>();
//builder.Services.AddSingleton(dbSettings);

#endregion

builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
//use the following if there is interface for the class you would like to inject it 
builder.Services.AddScoped(typeof(ProductService));
//but use the following if there is an interface
//builder.Services.AddScoped<IProductService, ProductService>();


// Add services to the container.
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
