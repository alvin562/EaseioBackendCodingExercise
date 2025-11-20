using Microsoft.EntityFrameworkCore;
using EaseioBackendCodingExercise.Data;
using EaseioBackendCodingExercise.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Register GUID service for dependency injection
builder.Services.AddScoped<GUIDService>();

// Configure SQLite
builder.Services.AddDbContext<GUIDDbContext>(options =>
    options.UseSqlite("Data Source=app.db")); // SQLite file app.db

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
