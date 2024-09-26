using AvansMealDeal.Infrastructure.Application.SQLServer;
using AvansMealDeal.Infrastructure.Identity.SQLServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
