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
