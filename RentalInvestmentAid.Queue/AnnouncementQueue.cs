using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentalInvestmentAid.Settings;
using System.Text;
using System.Threading.Channels;


namespace RentalInvestmentAid.Queue
{
    public static class AnnouncementQueue
    {

        private static ConnectionFactory factory = new ConnectionFactory { HostName = "localhost" };
        private static IConnection _connection ;
        private static IModel _channel;
        private static string _queueName = SettingsManager.AnnouncementQueueName;
        static AnnouncementQueue()
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
        }

        public static EventingBasicConsumer Consumer
        {
            get
            {                
                return new EventingBasicConsumer(_channel);
            }
        }

        public static void SetConsumer(EventingBasicConsumer consumer)
        {
            _channel.BasicConsume(queue: _queueName,
                     autoAck: true,
                     consumer: consumer);
        }
        public static void SendMessage(string url)
        {
            // Send the Order to the queue.
            Logger.LogHelper.LogInfo($"Send {url} to {_queueName}");
            var body = Encoding.UTF8.GetBytes(url);

            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
