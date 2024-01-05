//using MongoDB_API.Models;
//using MongoDB_API.Services;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.Configure<BeersDBSettings>(
//    builder.Configuration.GetSection("BeersDatabase"));


//builder.Services.AddSingleton<BeersService>();

//builder.Services.AddControllers();


//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();



using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB_API.Models;
using MongoDB_API.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<BeersDBSettings>(
    builder.Configuration.GetSection("BeersDatabase"));

builder.Services.AddSingleton<BeersService>();

builder.Services.AddControllers();

/*builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 5001; // Set the HTTPS port
});*/

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "MongoDB API",
        Version = "v1"
    });
});

// Always register the HttpClient, but conditionally configure it for development.
builder.Services.AddHttpClient("MyHttpClient")
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        if (builder.Environment.IsDevelopment())
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
                {
                    // Replace "YourTrustedThumbprint" with the actual thumbprint of your certificate.
                    return certificate != null && certificate.Thumbprint.Equals("ca15217ee355b19fcbfbeb82a15271ef0c54023f", StringComparison.OrdinalIgnoreCase);
                }
            };
        }

        return new HttpClientHandler();
    });



var app = builder.Build();
app.UseSwagger();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

