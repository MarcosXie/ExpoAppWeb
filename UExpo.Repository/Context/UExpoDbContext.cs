using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UExpo.Domain.Users;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Context;

public class UExpoDbContext : DbContext
{
    //public UExpoDbContext(DbContextOptions<UExpoDbContext> options) : base(options)
    //{
    //}

    public virtual DbSet<UserDao> Users { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    var converter = new ValueConverter<TypeEnum, string>(
    //            v => v.ToString(),
    //            v => (TypeEnum)Enum.Parse(typeof(TypeEnum), v)
    //        );

    //    modelBuilder.Entity<UserDao>()
    //        .Property(equals => equals.Type)
    //        .HasConversion(converter);
    //}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source=C:\\Users\\hsieh\\source\\repos\\UExpo\\UExpo.Repository\\UExpo.db");
    }
}
