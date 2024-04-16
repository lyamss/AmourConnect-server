using Microsoft.EntityFrameworkCore;
using server_api.Data;
using server_api.Interfaces;
using server_api.Models;
using server_api.Repository;
using DotNetEnv;


Env.Load();
Env.TraversePath().Load();


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Env.GetString("ConnectionDB"))
builder.Services.AddDbContext<ApiDbContext>(options =>
options.UseNpgsql(Env.GetString("ConnectionDB")));


var app = builder.Build();


// Migration
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    dataContext.Database.Migrate();
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApiDbContext>();
        var seedData = new SeedData();
        seedData.SeedApiDbContext(context);
    }
}


// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();