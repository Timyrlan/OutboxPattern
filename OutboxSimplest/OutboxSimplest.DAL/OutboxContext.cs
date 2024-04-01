using Microsoft.EntityFrameworkCore;

namespace OutboxSimplest.DAL;

public class OutboxContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Database=OutboxSimplest;Username=usr;Password=pwd");
    }

    /// <summary>
    /// Сообщения для отправки
    /// </summary>
    public DbSet<OutgoingMessage> OutgoingMessages { get; set; }
}