using Microsoft.EntityFrameworkCore;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Context;

public class UExpoDbContext : DbContext
{
    //public UExpoDbContext(DbContextOptions<UExpoDbContext> options) : base(options)
    //{
    //}

    public virtual DbSet<UserDao> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=UExpo.db");
    }
}
