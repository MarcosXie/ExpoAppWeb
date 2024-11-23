using ExpoShared.Repository.Context;
using ExpoShared.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExpoApp.Repository.Context;

public class ExpoAppDbContext(IConfiguration configuration) : ExpoSharedContext(configuration)
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.AddSharedTables();

		modelBuilder.IgnoreUExpoTables();
		
		base.OnModelCreating(modelBuilder);
	}
}