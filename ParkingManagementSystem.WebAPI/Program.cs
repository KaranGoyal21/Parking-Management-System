using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ParkingManagementSystem.Business.Interfaces;
using ParkingManagementSystem.Business.Repository;
using ParkingManagementSystem.Business.Services;
using ParkingManagementSystem.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Parking Management Service APIs", Version = "v1.0" });
});

builder.Services.AddDbContext<PmsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);

builder.Services.AddScoped<IParkingService, ParkingService>();
builder.Services.AddSingleton<IPmsRepository, SqlPmsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
