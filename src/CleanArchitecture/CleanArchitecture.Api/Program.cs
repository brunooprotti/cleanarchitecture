using CleanArchitecture.Infrastructure;
using CleanArchitecture.Application;
using CleanArchitecture.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.ApplyMigration();
app.SeedData();

app.UseCustomExceptionHandler();

app.MapControllers();

app.Run();