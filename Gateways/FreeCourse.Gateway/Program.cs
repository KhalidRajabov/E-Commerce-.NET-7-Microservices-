using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
builder.Services.AddOcelot();

public static IHostBuilder CreateHostBuilder(string[]args)=>
    Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile($"configuration.{hostingContext}")
    })

await app.UseOcelot();
app.Run();
