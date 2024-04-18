using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentalInvestmentAid.Settings;
using System.Text;
using System.Threading.Channels;


namespace RentalInvestmentAid.Queue
{
    public class RabbitMQBroker : IBroker
    {
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;
        public RabbitMQBroker(string queueName, string hostName = "localhost")
        {
            _queueName = queueName;
            _factory = new ConnectionFactory { HostName = hostName };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
        }

        public EventingBasicConsumer GetConsumer()
        {
            return new EventingBasicConsumer(_channel);
        }

        public  void SetConsumer(EventingBasicConsumer consumer)
        {
            _channel.BasicConsume(queue: _queueName,
                     autoAck: true,
                     consumer: consumer);
        }
        public void SendMessage<T>(T message)
        {
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
