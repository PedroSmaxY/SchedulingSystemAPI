using dotenv.net;
using Microsoft.EntityFrameworkCore;
using SchedulingSystemAPI.Data;
using SchedulingSystemAPI.Repositories;
using SchedulingSystemAPI.Repositories.Interfaces;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

connectionString = connectionString
    .Replace("${DB_SERVER}", Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost")
    .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "scheduling_system_db")
    .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "postgres")
    .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAvailableSlotRepository, AvailableSlotRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
