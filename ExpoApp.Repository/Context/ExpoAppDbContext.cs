using ExpoApp.Domain.Dao;
using ExpoApp.Repository.Configuration;
using ExpoShared.Repository.Context;
using ExpoShared.Repository.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ExpoApp.Repository.Context;

public class ExpoAppDbContext() : ExpoSharedContext()
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