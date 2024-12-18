using AvansMealDeal.Application.Services;
using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;
using AvansMealDeal.Infrastructure.Application.SQLServer;
using AvansMealDeal.Infrastructure.Application.SQLServer.Repositories;
using AvansMealDeal.Infrastructure.Identity.SQLServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// dependency injection domain services
builder.Services.AddTransient<ICanteenRepository, SqlServerCanteenRepository>();
builder.Services.AddTransient<IMealRepository, SqlServerMealRepository>();
builder.Services.AddTransient<IMealPackageRepository, SqlServerMealPackageRepository>();
builder.Services.AddTransient<IReservationRepository, SqlServerReservationRepository>();

// dependency injection application services
builder.Services.AddTransient<ICanteenService, CanteenService>();
builder.Services.AddTransient<IMealService, MealService>();
builder.Services.AddTransient<IMealPackageService, MealPackageService>();
builder.Services.AddTransient<IReservationService, ReservationService>();

// add databases
builder.Services.AddDbContext<DbContextApplicationSqlServer>(x => x.UseSqlServer(Environment.GetEnvironmentVariable("DATABASES_APPLICATION") ?? builder.Configuration.GetConnectionString("Databases_Application") ?? builder.Configuration.GetValue<string>("Databases:Application"),
    sqlServer => sqlServer.MigrationsAssembly("AvansMealDeal.Infrastructure.Application.SQLServer"))); // needed to deploy migrations
builder.Services.AddDbContext<DbContextIdentitySqlServer>(x => x.UseSqlServer(Environment.GetEnvironmentVariable("DATABASES_IDENTITY") ?? builder.Configuration.GetConnectionString("Databases_Identity") ?? builder.Configuration.GetValue<string>("Databases:Identity"),
    sqlServer => sqlServer.MigrationsAssembly("AvansMealDeal.Infrastructure.Identity.SQLServer"))); // needed to deploy migrations
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<MealDealUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DbContextIdentitySqlServer>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
