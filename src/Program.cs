using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using API.Services;
using StackExchange.Redis;
using API.Features.Authentification.Filters;
using API.Persistence;
using API.Seeders;
using API.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("EnvSecret.json", optional: false, reloadOnChange: true);
builder.Services.Configure<SecretEnv>(builder.Configuration.GetSection("EnvSecret"));
var envSecret = builder.Configuration.GetSection("EnvSecret").Get<SecretEnv>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddInfrastructure(envSecret.ConnexionDb, envSecret.ConnexionRedis);

builder.Services.AddScoped<AuthorizeAuth>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "web_site_Front", configurePolicy: policyBuilder =>
    {
        policyBuilder.WithOrigins(envSecret.IpFrontend);
        policyBuilder.WithHeaders("Content-Type");
        policyBuilder.WithMethods("GET", "POST", "PATCH");
        policyBuilder.AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = envSecret.OAuthGoogleClientId;
    options.ClientSecret = envSecret.OAuthGoogleClientSecret;
    options.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
    {
        OnRemoteFailure = context =>
        {
            context.Response.Redirect(envSecret.IpFrontend + "/login");
            context.HandleResponse();
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<IBackendDbContext>();
    dataContext.Migrate();

    var cacheDB = ConnectionMultiplexer.Connect(envSecret.ConnexionRedis);
    if (!cacheDB.IsConnected)
    {
        Console.WriteLine("Failed to connect to CacheDB, Exiting API :/");
        Environment.Exit(1);
    }
}

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<IUserSeeder>();
    await seeder.Seed();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors("web_site_Front");

await app.RunAsync();