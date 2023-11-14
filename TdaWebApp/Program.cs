using AspNetCore.Identity.MongoDbCore.Infrastructure;
using MongoDB.Driver;
using System.Configuration;
using TdaWebApp.Models;
using TdaWebApp.Services;
using TdaWebApp.Settings;

var builder = WebApplication.CreateBuilder(args);

/*// Add services to the container.
builder.Services.AddControllersWithViews();*/

// Add services to the container.
builder.Services.AddSingleton<BeersService>();
//
/*builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient("mongodb://localhost:27017"));
builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase("mydb");
});*/


var Configuration = builder.Configuration;
var mongoDbSettings = Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();

    builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
        (
            mongoDbSettings.ConnectionString, mongoDbSettings.Name
        );

//
/*builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);*/
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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