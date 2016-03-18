using System;
using System.Threading;
using CommandHandlerTest;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using Seven.Commands;
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

            var comsumer = new MessageConsumer(broker, binarySerializer, new DefaultJsonSerializer(), "", "");

            var commandService = new CommandService(producer, comsumer);
            
            var command = new CreateUserCommand()
            {
                Age = 22,
                UserName = "天涯狼" + DateTime.Now.ToString("yyyyMMddHHmmsss"),
                UserPassword = DateTime.Now.ToString("yyyyMMddHHmmsss")
            };

            var commandHandleResult = commandService.Send(command);

            Console.WriteLine("CommandHandleResult Message:{0};Status:{1}", commandHandleResult.Message, commandHandleResult.Status);
        }
    }
}
