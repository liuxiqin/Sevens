using System;
using System.Threading;
using CommandHandlerTest;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using Seven.Infrastructure.Serializer;
using Seven.Message;

namespace RabbitMQServerTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var binarySerializer = new DefaultBinarySerializer();
            
            var connectionInfo = new RabbitMqConnectionInfo("guest", "guest", "127.0.0.1", 5672);

            var broker = new MessageBroker(binarySerializer, connectionInfo);

            var producer = new MessageProducer(broker, binarySerializer, new DefaultJsonSerializer());

            while (true)
            {
                var command = new CreateUserCommand()
                {
                    Age = 22,
                    UserName = "天涯狼" + DateTime.Now.ToString("yyyyMMddHHmmsss"),
                    UserPassword = DateTime.Now.ToString("yyyyMMddHHmmsss")
                };

                producer.Publish(command);

                Console.WriteLine(command.ToString());

                Thread.Sleep(1);
            }
        }
    }
}
