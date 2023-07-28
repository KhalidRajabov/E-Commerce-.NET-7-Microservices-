using FreeCourse.Gateway.DelegateHandlers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<TokenExchangeDelegateHandler>();
builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", opt =>
{
    opt.Authority = builder.Configuration["IdentityServerURL"];
    opt.Audience = "resource_gateway";
    opt.RequireHttpsMetadata = false;
});
builder.Services.AddOcelot().AddDelegatingHandler<TokenExchangeDelegateHandler>();
var app = builder.Build();

builder.Configuration.AddJsonFile($"configuration.development.json");

await app.UseOcelot();
app.UseAuthorization();
app.Run();