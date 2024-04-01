using Microsoft.EntityFrameworkCore;
using Outbox1.DAL;

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

            Console.WriteLine($"{id} {SenderId} received Message.Sended={message.Sended}, message.Sender={message.Sender}");

            if (message.Sended)
            {
                Console.WriteLine($"{id} {SenderId} breaking Message.Sended={message.Sended}, message.Sender={message.Sender}");
                return;
            }

            //Для срабатывания DbUpdateConcurrencyException запись должна поменяться. Например, запишем инстанс отправщика
            message.Sender = SenderId.ToString();

            Console.WriteLine($"{id} {SenderId} locking Message.Sended={message.Sended}, message.Sender={message.Sender}");
            await context.SaveChangesAsync();

            await SendOut(message);
            message.Sended = true;

            Console.WriteLine($"{id} {SenderId} saving Message.Sended={message.Sended}, message.Sender={message.Sender}");
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // Ошибка здесь означает, что кто-то уже попытался отправить запись
            Console.WriteLine($"{id} {SenderId} DbUpdateConcurrencyException");
        }
        catch (Exception e)
        {
            Console.WriteLine($"{id} {SenderId} error " + e);
        }
    }

    public virtual async Task SendOut(OutgoingMessage message)
    {
        Console.WriteLine($"{message.Id} {SenderId} SENDED Message.Sended={message.Sended}, message.Sender={message.Sender}");
    }
}