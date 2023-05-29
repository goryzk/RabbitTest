using System.Text;
using RabbitMQ.Client;

namespace Publisher;

public class RabbitMqPublisher
{
    private readonly IConnection _connection;
    public RabbitMqPublisher()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        
        _connection = factory.CreateConnection();
    }
    
    public void Publish(string message)
    {
        // Create a channel
        using var channel = _connection.CreateModel();
        // Declare a queue
        channel.QueueDeclare(queue: "my-queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        // Publish a message
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "",
            routingKey: "my-queue",
            basicProperties: null,
            body: body);

        Console.WriteLine("Message sent: {0}", message);
    }
}