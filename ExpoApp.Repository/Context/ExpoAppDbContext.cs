using ExpoApp.Domain.Dao;
using ExpoApp.Repository.Configuration;
using ExpoShared.Repository.Context;
using ExpoShared.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExpoApp.Repository.Context;

public class ExpoAppDbContext(IConfiguration configuration) : ExpoSharedContext(configuration)
{
	public DbSet<MomentoDao> Momentos { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.AddSharedTables();
		modelBuilder.IgnoreUExpoTables();
		
		// Adds ExpoApp Tables
		modelBuilder.ApplyConfiguration(new MomentoConfiguration());
		
		base.OnModelCreating(modelBuilder);
	}
}