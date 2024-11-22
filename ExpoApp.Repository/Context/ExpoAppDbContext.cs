using ExpoShared.Domain.Dao;
using ExpoShared.Repository.Context;
using ExpoShared.Repository.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ExpoApp.Repository.Context;

public class ExpoAppDbContext : ExpoSharedContext
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.AddSharedTables();

		modelBuilder.IgnoreUExpoTables();
		
		base.OnModelCreating(modelBuilder);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseMySql(
			"server=uexpo-db.cbasq20g4rj4.us-east-1.rds.amazonaws.com;database=expoapp_db;user=root;password=RootAws123!",
			new MySqlServerVersion(new Version(8, 0, 37)));
	}
}