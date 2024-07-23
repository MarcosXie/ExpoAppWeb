using Microsoft.EntityFrameworkCore;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Context;

public class UExpoDbContext
{
    public UExpoDbContext() { }

    public virtual DbSet<UserDao> Users { get; set; }
}
