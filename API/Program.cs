using System.Reflection;
using Aplication.Activities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("defaultConnection")
));
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", policy =>
    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000")
));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(List).Assembly));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    context.Database.Migrate();
    await Seed.SeedData(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error has occurred during the migration");
}
app.Run();
