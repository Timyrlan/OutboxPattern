using Microsoft.EntityFrameworkCore;
using OutboxSimplest.DAL;

public class Sender
{
    public async Task Send()
    {
        var context = new OutboxContext();

        // получаем неотправленные сообщения
        var messages = await context.OutgoingMessages.Where(c => !c.Sended).ToArrayAsync();

        // отправляем
        foreach (var message in messages) await Send(message, context);
    }

    public async Task Send(
        OutgoingMessage message,
        OutboxContext context)
    {
        try
        {
            await SendOut(message);
            message.Sended = true;
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error processing message={message.Id}, " + e);
        }
    }

    public virtual async Task SendOut(OutgoingMessage message)
    {
        // Имитируем отправку в стороннюю систему
        Console.WriteLine($"SENDED message={message.Id}");
    }
}