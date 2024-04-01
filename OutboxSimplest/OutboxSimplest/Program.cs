using Microsoft.EntityFrameworkCore;
using OutboxSimplest.DAL;

//заполняем тестовые данные
using var context = new OutboxContext();
await context.Database.MigrateAsync();
for (var i = 0; i < 1000; i++) context.OutgoingMessages.Add(new OutgoingMessage());
await context.SaveChangesAsync();

// создаем отправщика
var sender = new Sender();

// отправляем
await sender.Send();