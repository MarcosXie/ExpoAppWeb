using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Repository.Configurations;

namespace UExpo.Repository.Context;

public class UExpoDbContext() : DbContext
{
    public virtual DbSet<UserDao> Users { get; set; }
    public virtual DbSet<UserImageDao> UserImages { get; set; }
    public virtual DbSet<CallCenterChatDao> CallCenterChats { get; set; }
    public virtual DbSet<CallCenterMessageDao> CallCenterMessages { get; set; }
    public virtual DbSet<AdminDao> Admins { get; set; }
    public virtual DbSet<CatalogDao> Catalogs { get; set; }
    public virtual DbSet<CatalogPdfDao> CatalogPdfs { get; set; }
    public virtual DbSet<CatalogItemImageDao> CatalogImages { get; set; }
    public virtual DbSet<AgendaDao> Agendas { get; set; }
    public virtual DbSet<FairDao> Fairs { get; set; }
    public virtual DbSet<SegmentDao> Segments { get; set; }
    public virtual DbSet<CalendarDao> Calendars { get; set; }
    public virtual DbSet<CalendarFairDao> CalendarFairs{ get; set; }
    public virtual DbSet<CalendarSegmentDao> CalendarSegments { get; set; }
    public virtual DbSet<TutorialDao> Tutorials { get; set; }
    public virtual DbSet<ExhibitorFairRegisterDao> FairRegisters { get; set; }
    public virtual DbSet<LastSearchedTagsDao> LastSearchedTags { get; set; }
    public virtual DbSet<RelationshipDao> Relationships { get; set; }
    public virtual DbSet<RelationshipMessageDao> RelationshipsMessages { get; set; }
	public virtual DbSet<CartDao> Carts { get; set; }
	public virtual DbSet<CartItemDao> CartItems { get; set; }
	public virtual DbSet<CartMessageDao> CartMessages { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=uexpo-db.cbasq20g4rj4.us-east-1.rds.amazonaws.com;database=uexpo_db;user=root;password=RootAws123!",
            new MySqlServerVersion(new Version(8, 0, 37)));
    }
}
