using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ExpoApp.Api.Hubs;
using ExpoApp.Api.Middlewares;
using ExpoApp.Application.Extensions;
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
        policy.WithOrigins("http://localhost:5174", "http://localhost:5173", "http://10.0.0.34:5173", "https://uexpo.com.br")
              .AllowAnyHeader()
              .AllowAnyMethod()
			  .WithExposedHeaders("Content-Disposition")
              .AllowCredentials();
    });
});

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UExpo API", Version = "v1" });

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
services.AddSharedRepository(config);

// Adding Expo App
services.AddApplication();
services.AddRepository(config);

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


//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//	//	// Define as op��es do Kestrel, como configurar o n�mero m�ximo de conex�es simult�neas.
//	//	serverOptions.Limits.MaxConcurrentConnections = 100; // N�mero m�ximo de conex�es simult�neas
//	//	serverOptions.Limits.MaxConcurrentUpgradedConnections = 100; // Para conex�es WebSockets

//	//	// Configure para escutar em uma porta espec�fica
//	//	//serverOptions.ListenAnyIP(int.Parse(config["Kestrel:Endpoints:Http:Url"] ?? "5003")); // Exemplo: HTTP na porta 5003
//	serverOptions.ListenAnyIP(443, listenOptions =>
//	{
//		listenOptions.UseHttps();
//	});
//});

WebApplication app = builder.Build();

// Obter o logger
var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("ENVIRONMENT VARIABLES:");

// Registrar todas as vari�veis de ambiente
foreach (var variable in Environment.GetEnvironmentVariables().Keys)
{
	var k = variable.ToString();
	var value = Environment.GetEnvironmentVariable(k);
	logger.LogInformation($"Environment Variable - {k}: {value}");
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "UExpo API v1");
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
