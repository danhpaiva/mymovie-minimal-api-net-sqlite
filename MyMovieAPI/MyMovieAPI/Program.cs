using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyMovieAPI.Data;
using MyMovieAPI.EndPoints;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddDbContext<MyMovieAPIContext>(options =>
    options
    .UseSqlite(builder
    .Configuration
    .GetConnectionString("MyMovieAPIContext") ?? throw new InvalidOperationException("Connection string 'MyMovieAPIContext' not found.")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapAtorEndpoints();

app.MapFilmeEndpoints();

app.MapFilmeAtorEndpoints();

app.MapAvaliacaoEndpoints();

app.Run();
