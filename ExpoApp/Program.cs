using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ExpoApp.Api.Hubs;
using ExpoApp.Api.Middlewares;
using ExpoApp.Application.Extensions;
using ExpoApp.Domain.Extensions;
using ExpoApp.Repository.Extensions;
using ExpoShared.Application.Extensions;
using ExpoShared.Application.Services.Users;
using ExpoShared.Application.Utils;
using ExpoShared.Domain.Dao;
using ExpoShared.Infrastructure.Extensions;
using ExpoShared.Repository.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IServiceCollection services = builder.Services;
ConfigurationManager config = builder.Configuration;
config.AddEnvironmentVariables();


builder.Services.AddLogging(loggingBuilder =>
{
	loggingBuilder.AddConsole(); // Configura o logger para sa�da no console
});

//CORS
services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
		        .AllowAnyOrigin()
	          // .WithOrigins("http://localhost:5174", "http://localhost:5173", "http://10.0.0.34:5173", "https://expoapp.com.br", "177.128.51.223", "172.31.0.73",  "http://177.128.51.223", "http://172.31.0.73",  "https://177.128.51.223", "https://172.31.0.73")
              .AllowAnyHeader()
              .AllowAnyMethod()
			  .WithExposedHeaders("Content-Disposition")
              // .AllowCredentials()
		        ;
    });
});

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExpoApp API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the Bearer token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme,
                }
            },
            Array.Empty<string>()
        }
    });
});

services.AddHttpContextAccessor();
services.AddSignalR(o =>
{
	o.EnableDetailedErrors = true;
	o.MaximumReceiveMessageSize = 10000000; // bytes
});

// Adding Shared 
services.AddSharedApplication();
services.AddSharedInfrastructure();
services.AddSharedRepository();

// Adding Expo App
services.AddDomain();
services.AddApplication();
services.AddRepository();

// Add authentication
byte[] key = Encoding.ASCII.GetBytes(config["Jwt:Key"]!);

services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
	x.RequireHttpsMetadata = false;
	x.SaveToken = true;
	x.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = true,
		ValidateActor = true,
		ValidIssuer = config["Jwt:Issuer"],
		ValidAudience = config["Jwt:Audience"]
	};
});

services.AddIdentityCore<UserDao>()
    .AddUserStore<UExpoUserStore>()
            .AddDefaultTokenProviders();

services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

WebApplication app = builder.Build();

// Obter o logger
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// logger.LogInformation("ENVIRONMENT VARIABLES:");

// // Registrar todas as vari�veis de ambiente
// foreach (var variable in Environment.GetEnvironmentVariables().Keys)
// {
// 	var k = variable.ToString();
// 	var value = Environment.GetEnvironmentVariable(k);
// 	logger.LogInformation($"Environment Variable - {k}: {value}");
// }

app.Use(async (context, next) =>
{
	var ip = context.Connection.RemoteIpAddress;
	logger.LogInformation($"Requisição de: {ip}");
	await next();
});

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExpoApp API v1");
});
//}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<CallCenterChatHub>("/call-center-chathub")
    .RequireAuthorization();

app.MapHub<RelationshipChatHub>("/relationship-chathub")
	.RequireAuthorization();

app.MapHub<CartChatHub>("/cart-chathub")
	.RequireAuthorization();

app.MapHub<NotificationsHub>("/notifications-hub")
	.RequireAuthorization();

//app.UseHttpsRedirection();

SeedDataHelper.BootstrapAdmin(app.Services);

app.Run();
