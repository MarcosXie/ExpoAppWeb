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
builder.Configuration.AddUserSecrets<Program>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 200 * 1024 * 1024; // Exemplo: 200MB
});


// Add services to the container.
IServiceCollection services = builder.Services;
ConfigurationManager config = builder.Configuration;
config.AddEnvironmentVariables();

// --- ALTERAÇÃO 1: LOG PARA CONFIRMAR O AMBIENTE ---
// Para usar o logger durante a configuração, precisamos criá-lo temporariamente.
var tempLogger = LoggerFactory.Create(config => config.AddConsole()).CreateLogger<Program>();

if (!builder.Environment.IsDevelopment())
{
    tempLogger.LogInformation("✅ Production environment detected. Loading configuration from AWS Parameter Store (/LoroApp)...");
    config.AddSystemsManager("/LoroApp");
}
else
{
    tempLogger.LogInformation("🛠️ Development environment detected. Skipping AWS Parameter Store. Using User Secrets and appsettings.json.");
}
// --- FIM DA ALTERAÇÃO 1 ---

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
});

//CORS
services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
              .WithOrigins("http://localhost:3000", "https://andre-e-leticia.vercel.app", "http://localhost:5174", "http://localhost:5173", "http://10.0.0.34:5173", "https://expoapp.com.br", "177.128.51.223", "172.31.0.73",  "http://177.128.51.223", "http://172.31.0.73",  "https://177.128.51.223", "https://172.31.0.73")
              .AllowAnyHeader()
              .AllowAnyMethod()
            .WithExposedHeaders("Content-Disposition")
              .AllowCredentials()
               ;
    });
});

services.AddControllers();
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
services.AddSharedRepository(false);

// Adds Firebase
services.AddFirebase(config);

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

var logger = app.Services.GetRequiredService<ILogger<Program>>();

// --- ALTERAÇÃO 2: VERIFICAÇÃO SEGURA DAS CONFIGURAÇÕES CARREGADAS ---
logger.LogInformation("--- Verificando Configurações Carregadas ---");

// Função auxiliar para registrar o status de uma chave de forma segura
void LogConfigurationStatus(string key)
{
    var value = app.Configuration[key];
    if (string.IsNullOrEmpty(value))
    {
        // Se a chave não for encontrada, emite um aviso.
        logger.LogWarning("⚠️ CONFIG KEY '{Key}' NÃO ENCONTRADA ou está vazia.", key);
    }
    else
    {
        // NÃO LOGUE O VALOR! Apenas confirme que foi carregado com sucesso.
        logger.LogInformation("✔️ CONFIG KEY '{Key}' carregada com sucesso.", key);
    }
}

// Verifique todas as suas chaves importantes aqui
LogConfigurationStatus("AzureSpeech:SubscriptionKey");
LogConfigurationStatus("AzureSpeech:Region");
LogConfigurationStatus("AzureTranslator:SubscriptionKey");
LogConfigurationStatus("AzureTranslator:Endpoint");
LogConfigurationStatus("Jwt:Key");

logger.LogInformation("-------------------------------------------");
// --- FIM DA ALTERAÇÃO 2 ---


app.Use(async (context, next) =>
{
    var ip = context.Connection.RemoteIpAddress;
    logger.LogInformation($"Requisição de: {ip}");
    await next();
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExpoApp API v1");
});

app.UseMiddleware<ExceptionMiddleware>();
app.UseWebSockets();
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
app.MapHub<GroupChatHub>("/group-chathub")
    .RequireAuthorization();
app.MapHub<CartChatHub>("/cart-chathub")
    .RequireAuthorization();
app.MapHub<NotificationsHub>("/notifications-hub")
    .RequireAuthorization();

SeedDataHelper.BootstrapAdmin(app.Services);

app.Run();