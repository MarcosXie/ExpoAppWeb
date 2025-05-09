using ExpoApp.Domain.Dao;
using ExpoApp.Repository.Configuration;
using ExpoShared.Repository.Context;
using ExpoShared.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ExpoShared.Repository.Configurations;

namespace ExpoApp.Repository.Context;

public class ExpoAppDbContext : ExpoSharedContext
{
    public ExpoAppDbContext() : this(BuildConfiguration())
    {
    }

    public ExpoAppDbContext(IConfiguration configuration) : base(configuration)
    {
    }

    private static IConfiguration BuildConfiguration()
    {
        // Calcula o caminho base subindo um nível e entrando na pasta ExpoApp.Api
        var currentDirectory = Directory.GetCurrentDirectory();
        var parentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        if (string.IsNullOrEmpty(parentDirectory))
        {
            throw new InvalidOperationException("Não foi possível determinar o diretório pai do diretório atual.");
        }

        var basePath = Path.Combine(parentDirectory, "ExpoApp");
        var appsettingsPath = Path.Combine(basePath, "appsettings.json");

        // Verifica se o arquivo existe
        if (!File.Exists(appsettingsPath))
        {
            throw new FileNotFoundException($"O arquivo appsettings.json não foi encontrado no caminho: {appsettingsPath}");
        }

        Console.WriteLine($"Usando appsettings.json em: {appsettingsPath}");

        return new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public DbSet<MomentoDao> Momentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddSharedTables();
        modelBuilder.IgnoreUExpoTables();
        
        // Adds ExpoApp Tables
        modelBuilder.ApplyConfiguration(new MomentoConfiguration());
        modelBuilder.ApplyConfiguration(new GroupConfiguration());
        modelBuilder.ApplyConfiguration(new GroupUserConfiguration());
        modelBuilder.ApplyConfiguration(new GroupMessageConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}