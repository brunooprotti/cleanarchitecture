using CleanArchitecture.Infrastructure;
using CleanArchitecture.Application;
using CleanArchitecture.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CleanArchitecture.Api.Configurations;
using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Infrastructure.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//registro el esquema de autenticación
    .AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddTransient<IJwtProvider,JwtProvider>();

builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();