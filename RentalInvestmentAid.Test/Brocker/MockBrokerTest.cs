using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using RentalInvestmentAid.Test.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RentalInvestmentAid.Test.Brocker
{

    [TestClass]
    public class MockBrokerTests
    {
        [TestMethod]
        public void SendMessage_ShouldStoreMessage()
        {
            // Arrange
            var mockBroker = new MockBroker();
            var testMessage = new { Text = "Hello, World!" };

            // Act
            mockBroker.SendMessage(testMessage);

            // Assert
            //Assert.Single(mockBroker.SentMessages);
            Assert.AreEqual(testMessage, mockBroker.SentMessages[0]);
        }



        [TestMethod]
        public void SendMessage_ShouldRaiseMessageSentEvent()
        {
            // Arrange
            var mockBroker = new MockBroker();
            var testMessage = new { Text = "Hello, World!" };
            bool eventRaised = false;

            mockBroker.MessageSent += (sender, e) =>
            {
                eventRaised = true;
                var sentMessage = JsonConvert.SerializeObject(e.Message);
                var expectedMessage = JsonConvert.SerializeObject(testMessage);
                Assert.AreEqual(expectedMessage, sentMessage);
            };

            // Act
            mockBroker.SendMessage(testMessage);

            // Assert
            Assert.IsTrue(eventRaised);
        }
    }
}
