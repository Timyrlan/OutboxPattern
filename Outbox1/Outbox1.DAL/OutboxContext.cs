using Microsoft.EntityFrameworkCore;

namespace Outbox1.DAL;

public class OutboxContext : DbContext
{
    public OutboxContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Database=Outbox1;Username=usr;Password=pwd;Application Name=Outbox1;");
    }

    public DbSet<OutgoingMessage> OutgoingMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OutgoingMessage>().Property(c => c.Version).IsRowVersion();
    }
}