using FreeCourse.Service.Discount.Services;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();

// Add services to the container.
//creates authorization requirement. ---In order to use, user must be signed in---
var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

//when we send token, token maps everything to "claims", changing some of their names,
//if we dont want any of the user info's not to be changed, we write the code below

//in here, the "sub" key will not be mapped to "nameidentifyer" property. it will remain with "sub" name
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "resource_discount";
    options.RequireHttpsMetadata = false;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();