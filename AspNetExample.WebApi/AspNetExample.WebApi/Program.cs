using System.Security.Claims;
using AspNetExample.Domain;
using AspNetExample.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<AspNetExample.Service.IGuidProvider, AspNetExample.Service.GuidProvider>();
builder.Services.AddTransient<AspNetExample.Shared.IDateTimeProvider, AspNetExample.Shared.DateTimeProvider>();

builder.Services.AddTransient<AspNetExample.Domain.Repositories.IRepository, AspNetExample.Domain.Repositories.Repository>();
builder.Services.AddTransient<AspNetExample.Service.Services.IService, AspNetExample.Service.Services.Service>();
builder.Services.AddTransient<AspNetExample.Service.IModelDescriptionProvider, AspNetExample.Service.ModelDescriptionProvider>();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
});
builder.Services.RegisterDomain();
builder.Services.RegisterService();
builder.Services.AddTransient<AspNetExample.Shared.IDateTimeProvider, AspNetExample.Shared.DateTimeProvider>();
builder.Services.AddTransient<AspNetExample.Service.IModelDescriptionProvider, AspNetExample.Service.ModelDescriptionProvider>();
builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    })
    .AddGoogle(x =>
    {
        x.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        x.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
        x.CallbackPath = "/signin-google";
        x.SaveTokens = true;
        x.Events = new()
        {
            OnTicketReceived = t =>
            {
                t.Principal?.AddIdentity(new([new("MyClaim", "MyValue")]));

                return Task.CompletedTask;
            }
        };
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();