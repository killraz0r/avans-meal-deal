using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Application.Services;
using AvansMealDeal.Domain.Services.Repositories;
using AvansMealDeal.Infrastructure.Application.SQLServer;
using AvansMealDeal.Infrastructure.Application.SQLServer.Repositories;
using AvansMealDeal.Infrastructure.Identity.SQLServer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

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
builder.Services.AddDbContext<DbContextApplicationSqlServer>(x => x.UseSqlServer(builder.Configuration.GetValue<string>("Databases:Application"), sqlServer => sqlServer.MigrationsAssembly("AvansMealDeal.Infrastructure.Application.SQLServer")));
builder.Services.AddDbContext<DbContextIdentitySqlServer>(x => x.UseSqlServer(builder.Configuration.GetValue<string>("Databases:Identity"), sqlServer => sqlServer.MigrationsAssembly("AvansMealDeal.Infrastructure.Identity.SQLServer")));

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

app.Run();
