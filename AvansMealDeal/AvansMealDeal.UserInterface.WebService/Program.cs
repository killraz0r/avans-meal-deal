using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Application.Services;
using AvansMealDeal.Domain.Services.Repositories;
using AvansMealDeal.Infrastructure.Application.SQLServer;
using AvansMealDeal.Infrastructure.Application.SQLServer.Repositories;
using AvansMealDeal.Infrastructure.Identity.SQLServer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using AvansMealDeal.UserInterface.WebService.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// configure the api so that the returned json does not have object cycles
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// dependency injection domain services
builder.Services.AddTransient<ICanteenRepository, SqlServerCanteenRepository>();
builder.Services.AddTransient<IMealPackageRepository, SqlServerMealPackageRepository>();
builder.Services.AddTransient<IReservationRepository, SqlServerReservationRepository>();

// dependency injection application services
builder.Services.AddTransient<IMealPackageService, MealPackageService>();
builder.Services.AddTransient<IReservationService, ReservationService>();

// add databases
builder.Services.AddDbContext<DbContextApplicationSqlServer>(x => x.UseSqlServer(builder.Configuration.GetValue<string>("Databases:Application") ?? builder.Configuration.GetConnectionString("Databases_Application") ?? Environment.GetEnvironmentVariable("DATABASES_APPLICATION"),
    sqlServer => sqlServer.MigrationsAssembly("AvansMealDeal.Infrastructure.Application.SQLServer"))); // needed to deploy migrations
builder.Services.AddDbContext<DbContextIdentitySqlServer>(x => x.UseSqlServer(builder.Configuration.GetValue<string>("Databases:Identity") ?? builder.Configuration.GetConnectionString("Databases_Identity") ?? Environment.GetEnvironmentVariable("DATABASES_IDENTITY"),
    sqlServer => sqlServer.MigrationsAssembly("AvansMealDeal.Infrastructure.Identity.SQLServer"))); // needed to deploy migrations

GraphQL.AddGraphQL(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGraphQL("/graphql");

app.Run();
