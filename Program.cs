using dotenv.net;
using Microsoft.EntityFrameworkCore;
using SchedulingSystemAPI.Data;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

connectionString = connectionString
    .Replace("${DB_SERVER}", Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost")
    .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "scheduling_system_db")
    .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "root")
    .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "");

// Add services to the container.
// Configuração para usar PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddControllers();
builder.Services.AddOpenApi();

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
