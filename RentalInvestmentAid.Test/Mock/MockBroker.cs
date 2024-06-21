using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RentalInvestmentAid.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Test.Mock
{
    public class MockBroker : IBroker
    {
        private EventingBasicConsumer _consumer;
        private readonly List<object> _data = new List<object>();
        public event EventHandler<MessageSentEventArgs> MessageSent;
        public EventingBasicConsumer GetConsumer()
        {
            return _consumer;
        }

        public void SendMessage<T>(T message)
        {
            _data.Add(message);

            OnMessageSent(new MessageSentEventArgs { Message = message });
        }
        public void SetConsumer(EventingBasicConsumer consumer)
        {
            _consumer = consumer;
        }

        protected virtual void OnMessageSent(MessageSentEventArgs e)
        {
            MessageSent?.Invoke(this, e);
        }

        public IReadOnlyList<object> SentMessages => _data.AsReadOnly();
    }
    public class MessageSentEventArgs : EventArgs
    {
        public object Message { get; set; }
    }
}
