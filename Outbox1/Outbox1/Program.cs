using Microsoft.EntityFrameworkCore;
using Outbox1.DAL;

//Для простоты примера best practice не используются, итоговое решение подведем в конце

//заполняем тестовые данные
using var context = new OutboxContext();
await context.Database.MigrateAsync();
for (var i = 0; i < 1000; i++) context.OutgoingMessages.Add(new OutgoingMessage());
await context.SaveChangesAsync();

var sender1 = new Sender();
var sender2 = new Sender();

Task.WaitAll(new[] { sender1.Send(), sender2.Send() });

public class Sender
{
    public Guid SenderId { get; } = Guid.NewGuid();

    public async Task Send()
    {
        var context = new OutboxContext();
        var ids = await context.OutgoingMessages.Where(c => !c.Sended).Select(c => c.Id).ToArrayAsync();

        foreach (var id in ids) await Send(id);
    }

    public async Task Send(int id)
    {
        try
        {
            using var context = new OutboxContext();
            var message = await context.OutgoingMessages.FirstAsync(c => c.Id == id);
            await SendOut(message);
            message.Sended = true;
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            Console.WriteLine($"DbUpdateConcurrencyException message={id}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error processing message={id}, " + e);
        }
    }

    /// <summary>
    /// Имитируем отправку в стороннюю систему
    /// </summary>
    public virtual async Task SendOut(OutgoingMessage message)
    {
        Console.WriteLine($"SENDED message={message.Id} by sender={SenderId}");
    }
}