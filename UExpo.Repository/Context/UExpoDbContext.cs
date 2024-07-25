using Microsoft.EntityFrameworkCore;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Context;

public class UExpoDbContext : DbContext
{
    public virtual DbSet<UserDao> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=uexpo-db.cbasq20g4rj4.us-east-1.rds.amazonaws.com;database=uexpo_db;user=root;password=RootAws123!",
            new MySqlServerVersion(new Version(8, 0, 37)));
    }
}
