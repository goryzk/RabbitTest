using Publisher;

RabbitMqPublisher publisher = new RabbitMqPublisher();
var messages = new List<string>
{
    "a", "b", "c", "ab", "dd", "ttt", "ac", "iop", "pok", "aav"
};
foreach (var item in messages)
{
    publisher.Publish(item);
}

Console.ReadKey();
