using Elfie.Serialization;
using FluentValidation.AspNetCore;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Extensions;
using FreeCourse.Web.Handlers;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services;
using FreeCourse.Web.Services.Interfaces;
using FreeCourse.Web.Validators;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews().AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<CourseCreateInputValidator>();
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddAccessTokenManagement();
builder.Services.AddSingleton<PhotoHelper>();
builder.Services.Configure<ServiceApiSettings>(builder.Configuration.GetSection("ServiceApiSettings"));

builder.Services.AddScoped<ResourceOwnerPasswordTokenHandler>();
builder.Services.AddScoped<ClientCredentialTokenHandler>();

//it substitutes scope service, because IdentitySservice class has httpclient used in its constructor
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();

builder.Services.AddHttpClientServices(builder.Configuration);


//reading configuration from appsettings or another json file and implementing it is called "Options pattern"
builder.Services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    CookieAuthenticationDefaults.AuthenticationScheme, opts=>{
        opts.LoginPath = "/Auth/Signin";
        opts.ExpireTimeSpan = TimeSpan.FromDays(60);
        opts.SlidingExpiration = true;
        opts.Cookie.Name = "udemywebcookie";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
