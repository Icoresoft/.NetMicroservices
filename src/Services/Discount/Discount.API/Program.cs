using Core.Data;
using Core.Extensions;
using Discount.API.Repositories;
using Discount.API.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Map and Inject AppSettings Section
//use this method to be able to access object of type DbSettings from ServiceProvider object
//1:
IDbSettings dbSettings = builder.Configuration.GetSection(nameof(DbSettings)).Get<DbSettings>();
builder.Services.AddSingleton(dbSettings);
//2:
builder.Services.AddNpgsqlConnection();
//3:
builder.Services.AddScoped<ICouponRepository,CouponRepository>();
//4:
builder.Services.AddScoped(typeof(CouponService));

var app=builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

//extension method to Migrate Postgres database
app.MigratePGDatabase<Program>();

app.Run();
