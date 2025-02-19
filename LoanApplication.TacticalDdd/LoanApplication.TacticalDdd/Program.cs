using Carter;
using LoanApplication.TacticalDdd.Application;
using LoanApplication.TacticalDdd.PortsAdapters.DataAccess;
using LoanApplication.TacticalDdd.PortsAdapters.ExternalServices;
using LoanApplication.TacticalDdd.PortsAdapters.MessageQueue;
using LoanApplication.TacticalDdd.PortsAdapters.Security;
using LoanApplication.TacticalDdd.ReadModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
builder.Services.AddAuthorization();
builder.Services.AddEfDbAdapters(builder.Configuration.GetConnectionString("LoanDb"));
builder.Services.AddRabbitMqClient("host=localhost");
builder.Services.AddExternalServicesClients();
builder.Services.AddApplicationServices();
builder.Services.AddReadModelServices(builder.Configuration.GetConnectionString("LoanDb"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",  
        Type = SecuritySchemeType.Http,  
        Scheme = "basic",  
        In = ParameterLocation.Header,  
        Description = "Basic Auth"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id="basic"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddCarter();

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapCarter();

app.Run();