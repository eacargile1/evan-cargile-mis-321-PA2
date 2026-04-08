using System.Text;
using LTS.Api;
using LTS.Core;
using LTS.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

HerokuConfig.UseHerokuPort(builder);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var connectionString = HerokuConfig.TryMysqlConnectionFromAddonUrl()
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "No database: set JAWSDB_URL / DATABASE_URL (mysql) on Heroku, or ConnectionStrings:DefaultConnection in config.");
var envPwd = Environment.GetEnvironmentVariable("LTS_MYSQL_PASSWORD");
if (!string.IsNullOrWhiteSpace(envPwd))
{
    var cb = new MySqlConnectionStringBuilder(connectionString) { Password = envPwd };
    connectionString = cb.ConnectionString;
}

builder.Services.AddDbContext<LTSDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITrainerProfileRepository, TrainerProfileRepository>();
builder.Services.AddScoped<IOfferingRepository, OfferingRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();

var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "lts-dev-secret-key-change-in-prod-!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = true,
            ValidIssuer = "lts-api",
            ValidateAudience = true,
            ValidAudience = "lts-client",
            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseCors();
if (!HerokuConfig.IsHerokuDyno)
    app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LTSDbContext>();
    db.Database.EnsureCreated();
    await DataSeeder.SeedAsync(db);
}

await app.RunAsync();
