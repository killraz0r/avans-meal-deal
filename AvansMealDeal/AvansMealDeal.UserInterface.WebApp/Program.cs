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

// dependency injection application services
builder.Services.AddTransient<ICanteenService, CanteenService>();
builder.Services.AddTransient<IMealService, MealService>();
builder.Services.AddTransient<IMealPackageService, MealPackageService>();

// add databases
builder.Services.AddDbContext<DbContextApplicationSqlServer>(x => x.UseSqlServer(builder.Configuration.GetValue<string>("Databases:Application")));
builder.Services.AddDbContext<DbContextIdentitySqlServer>(x => x.UseSqlServer(builder.Configuration.GetValue<string>("Databases:Identity")));
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
