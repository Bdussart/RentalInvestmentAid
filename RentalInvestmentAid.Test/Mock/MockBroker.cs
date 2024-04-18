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
        public EventingBasicConsumer GetConsumer()
        {
            throw new NotImplementedException();
        }

        public void SendMessage<T>(T message)
        {
            throw new NotImplementedException();
        }

        public void SetConsumer(EventingBasicConsumer consumer)
        {
            throw new NotImplementedException();
        }
    }
}
