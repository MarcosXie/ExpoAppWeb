using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UExpo.Api.Hubs;
using UExpo.Api.Middlewares;
using UExpo.Application.Extensions;
using UExpo.Application.Utils;
using UExpo.Domain.Dao;
using UExpo.Infrastructure.Extensions;
using UExpo.Repository.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IServiceCollection services = builder.Services;
ConfigurationManager config = builder.Configuration;

if (!config.GetValue<bool>("IsDev"))
{
    string port = Environment.GetEnvironmentVariable("PORT") ?? "5000";

    if (!string.IsNullOrEmpty(port))
    {
        builder.WebHost.UseKestrel().UseUrls($"http://*:{port}");
    }
}

config.AddEnvironmentVariables();

services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5174", config["FrontEndUrl"]!, "http://10.0.0.34:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
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
services.AddSignalR();
services.AddRepository(config);
services.AddInfrastructure();
services.AddApplication();

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

app.MapHub<CallCenterChatHub>("/relationship-chathub")
	.RequireAuthorization();

SeedDataHelper.BootstrapAdmin(app.Services);

app.Run();
