using HealthChecks.UI.Configuration;
using Microsoft.IdentityModel.Logging;
using Nexus.Authentication.WebAPI.Data;
using Nexus.Authentication.WebAPI.Scope.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddCustomLogging();

builder.Services.AddCustomDatabase();
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddCustomSwagger();
builder.Services.AddCustomControllers();
builder.Services.AddCustomExceptions();
builder.Services.AddCustomHealthChecks(builder.Configuration);
builder.Services.AddCustomServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await DatabaseInitializer.Migrate(serviceProvider);
    await DatabaseInitializer.Seed(serviceProvider);
}

if (app.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;
    app.UseCustomSwagger();
}

app.UseCustomAuthentication();
app.UseCustomControllers();
app.UseCustomExceptions();
app.UseCustomHealthChecks();

app.Run();
