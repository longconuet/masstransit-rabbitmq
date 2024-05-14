using Consumer;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddMassTransit(x =>
{
    x.AddConsumer<ItemCreatedMessageConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var serviceProvider = services.BuildServiceProvider();
var bus = serviceProvider.GetRequiredService<IBusControl>();
await bus.StartAsync();

Console.Write("Enter queue name: ");
var queue = Console.ReadLine();

Console.WriteLine("Waiting for messages...");
Console.ReadLine();