//using AspNetCore.Identity.MongoDbCore.Infrastructure;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using MongoDB.Driver;
//using TdaWebApp.Models;
//using TdaWebApp.Services;
//using TdaWebApp.Settings;

//var builder = WebApplication.CreateBuilder(args);

///*// Add services to the container.
//builder.Services.AddControllersWithViews();*/

//// Add services to the container.
//builder.Services.AddSingleton<BeersService>();

////builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient("mongodb+srv://yangzhe:Admin123@cluster0.ekqp6ae.mongodb.net/"));
////builder.Services.AddScoped<IMongoDatabase>(sp =>
////{
////    var client = sp.GetRequiredService<IMongoClient>();
////    return client.GetDatabase("mydb");
////});

//var Configuration = builder.Configuration;
//var mongoDbSettings = Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();

//if (mongoDbSettings == null)
//{
//    // Log or handle the case where configuration is not loaded correctly.
//    Console.WriteLine("MongoDbConfig section is null. Check your configuration.");
//    // You may also throw an exception or return an error response.
//}
//else
//{
//    builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
//        .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
//        (
//            mongoDbSettings.ConnectionString, mongoDbSettings.Name
//        );
//}

//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages(); //
//builder.Services.AddMvc(); //


//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();



//using AspNetCore.Identity.MongoDbCore.Infrastructure;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using TdaWebApp.Models;
//using TdaWebApp.Services;
//using TdaWebApp.Settings;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddSingleton<BeersService>();

//var Configuration = builder.Configuration;
//var mongoDbSettings = Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();

//if (mongoDbSettings == null)
//{
//    Console.WriteLine("MongoDbConfig section is null. Check your configuration.");
//}
//else
//{
//    builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
//        .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(
//            mongoDbSettings.ConnectionString, mongoDbSettings.Name
//        );

//    builder.Services.AddScoped<UserManager<ApplicationUser>>();
//    builder.Services.AddScoped<SignInManager<ApplicationUser>>();
//}

//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");


//app.Run();

using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using TdaWebApp.Models;
using TdaWebApp.Services;
using TdaWebApp.Settings;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

// Configure MongoDB settings
var mongoDbSettings = Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
if (mongoDbSettings == null)
{
    Console.WriteLine("MongoDbConfig section is null. Check your configuration.");
}
else
{
    // Add MongoDB configuration to the BeersService
    builder.Services.AddSingleton<IMongoDbConfig>(mongoDbSettings);
    builder.Services.AddSingleton<BeersService>();
}

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(mongoDbSettings.ConnectionString, mongoDbSettings.Name);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddMvc();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
