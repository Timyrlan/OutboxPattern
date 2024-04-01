using Microsoft.EntityFrameworkCore;
using Outbox1.DAL;

//Для простоты примера best practice не используются, итоговое решение подведем в конце

//заполняем тестовые данные
using var context = new OutboxContext();
await context.Database.MigrateAsync();
for (var i = 0; i < 100; i++) context.OutgoingMessages.Add(new OutgoingMessage());
await context.SaveChangesAsync();


Task.WaitAll(new[] { new Sender().Send(), new Sender().Send(), new Sender().Send(), new Sender().Send(), new Sender().Send() });