using Microsoft.EntityFrameworkCore;

public class MobileContext : DbContext
{
    public MobileContext(DbContextOptions<MobileContext> options)
        : base(options)
    {
    }

    public DbSet<Phone> Phones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Phone>().Property(p => p.Name).IsRequired();
        modelBuilder.Entity<Phone>().Property(p => p.Company).IsRequired();
        modelBuilder.Entity<Phone>().Property(p => p.Price).IsRequired();
        modelBuilder.Entity<Phone>().Property(p => p.DescriptionFile).IsRequired(false);
    }
}