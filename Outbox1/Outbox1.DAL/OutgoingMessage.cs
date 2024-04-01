namespace Outbox1.DAL;

public class OutgoingMessage
{
    public int Id { get; set; }

    public bool Sended { get; set; }
    public string? Sender { get; set; }

    /// <summary>
    /// Версия записи в БД. Меняется при каждом изменении записи
    /// </summary>
    public uint Version { get; set; }
}