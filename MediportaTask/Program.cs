using MediportaTask;
using MediportaTask.Data.Contexts;
using MediportaTask.HostedServices.FetchSOTags;
using MediportaTask.Misc;
using MediportaTask.Misc.Modules;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.ConfigureKestrel(b =>
//{
//    b.Listen(IPAddress.Any, 5000);
//});

builder.Services.AddControllers();

builder.Services.AddLogging(logger => logger.AddConsole());

builder.Services
    .AddDbContext<MediportaDbContext>(options => options
        .UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.RegisterModules();

builder.Services.AddHostedService<FetchSOTagsHostedService>();

builder.Services.Configure<MediportaConfig>(builder.Configuration.GetSection("Config"));

builder.Services.AddHttpClient("stackoverflow").ConfigurePrimaryHttpMessageHandler(opts => new HttpClientHandler
{
    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<MediportaDbContext>();
await dbContext.Database.MigrateAsync();

app.UseCors(o =>
{
    o.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});

//need certificate or reverse proxy setup to run http in a closed network
//app.UseHttpsRedirection();

app.UseAuthorization();

app.AddEndpoints();

app.Run();