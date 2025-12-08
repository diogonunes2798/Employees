using Employees.Api.Auth;
using Employees.Api.Converters;
using Employees.Api.Mapping;
using Employees.Api.Swagger;
using Employees.Application;
using Employees.Application.Database;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

//Forçar mensagens e cultura para Inglês
var culture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddControllers()
     .AddJsonOptions(options =>
      {
          options.JsonSerializerOptions.Converters.Add(new JsonDateOnlyConverter());
      });
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddCustomSwagger();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var connectionString = config["Database:ConnectionString"]!;
builder.Services.AddSingleton<IDbConnectionFactory>(new SqliteConnectionFactory(connectionString));
builder.Services.AddSingleton<DbInitializer>();


builder.Services.AddApplication();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    await initializer.InitializeAsync();
}

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
