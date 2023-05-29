using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer;

public class RabbitMqConsumer
{
    private readonly IConnection _connection;
    public RabbitMqConsumer()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        
        _connection = factory.CreateConnection();
    }
    public void Consume()
    {
        // Create a channel
        using var channel = _connection.CreateModel();
        // Declare a queue
        channel.QueueDeclare(queue: "my-queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        // Create a consumer
        var consumer = new EventingBasicConsumer(channel);

        // Handle received messages
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            if (message.Length > 2)
            {
                channel.BasicAck(ea.DeliveryTag, false);
                Console.WriteLine("Message received: {0}", message);
            }
            else
            {
                Console.WriteLine("Message still here: {0}", message);
            }
        };

        // Start consuming messages
        channel.BasicConsume(queue: "my-queue",
            autoAck: false,
            consumer: consumer);


        Console.WriteLine("Press [enter] to exit.");
        Console.ReadLine();
    }
}