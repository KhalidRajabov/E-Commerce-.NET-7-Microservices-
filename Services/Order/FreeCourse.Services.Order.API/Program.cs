using FreeCourse.Services.Order.Application.Consumers;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Messages;
using FreeCourse.Shared.Services;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateOrderMessageCommandConsumer>();
    x.AddConsumer<CourseNameChangedEventConsumer>();
    // Default Port : 5672
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQUrl"], "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
        //line below means, this api will read datas only from "create-order-service" tail of rabbitmq
        //after reading, if there are data waiting in tail, it will add them to the database.
        cfg.ReceiveEndpoint("create-order-service", e =>
        {
            e.ConfigureConsumer<CreateOrderMessageCommandConsumer>(context);
        });
        cfg.ReceiveEndpoint("course-name-changed-event-order-service", e =>
        {
            e.ConfigureConsumer<CourseNameChangedEventConsumer>(context);
        });
        
    });
});



//creates authorization requirement. ---In order to use, user must be signed in---
var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

//when we send token, token maps everything to "claims", changing some of their names,
//if we dont want any of the user info's not to be changed, we write the code below

//in here, the "sub" key will not be mapped to "nameidentifyer" property. it will remain with "sub" name
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "resource_order";
    options.RequireHttpsMetadata = false;
});

builder.Services.AddDbContext<OrderDbContext>(opt =>
{
    //code below means migration folder will be created in another project
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), cnfg =>
    {
        cnfg.MigrationsAssembly("FreeCourse.Services.Order.Infrastructure");
    });
});
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.AddMediatR(typeof(FreeCourse.Services.Order.Application.Handlers.CreateOrderCommandHandler).Assembly);
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
