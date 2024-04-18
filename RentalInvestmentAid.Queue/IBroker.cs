using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace RentalInvestmentAid.Queue
{
    public interface IBroker
    {
        public EventingBasicConsumer GetConsumer();

        public void SetConsumer(EventingBasicConsumer consumer);

        public void SendMessage<T>(T message);        

    }
}
