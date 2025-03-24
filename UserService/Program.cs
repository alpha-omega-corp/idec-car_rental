using CarRental.Repositories;
using System.Reflection;
using System.Text;
using CarRental.Database;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<Connection>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(config =>
        config.AddPostgres()
            .WithGlobalConnectionString($"Server={Configuration.GetEnvironment("DB_HOST")};Database={Configuration.GetEnvironment("DB_NAME")};User Id={Configuration.GetEnvironment("DB_USER")};Password={Configuration.GetEnvironment("DB_PASSWORD")};")
            .ScanIn(Assembly.GetExecutingAssembly()).For.All()
    ).AddLogging(config => config.AddFluentMigratorConsole());

builder.Services.AddAuthentication(cfg => {
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new TokenValidationParameters() {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8
                .GetBytes(Configuration.GetEnvironment("JWT_SECRET"))
        ),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    var runner = app.Services.CreateScope().ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateDown(0);
    runner.MigrateUp();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
