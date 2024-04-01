using Microsoft.EntityFrameworkCore;

namespace Outbox1.DAL;

public class OutboxContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Database=Outbox1;Username=usr;Password=pwd");
    }

    /// <summary>
    /// Сообщения для отправки
    /// </summary>
    public DbSet<OutgoingMessage> OutgoingMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Указываем билдеру, что необходимо использовать механизм RowVersion на уровне базы данных
        modelBuilder.Entity<OutgoingMessage>().Property(c => c.Version).IsRowVersion();
    }
}