using System;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using Seven.Commands;
using Seven.Infrastructure.Serializer;
using Seven.Messages;
using Seven.Messages.Channels;
using Seven.Messages.QueueMessages;
using Seven.Tests.UserSample.Commands;

namespace RabbitMQServerTest
{
    internal class Program
    {

        private static void Main(string[] args)
        {

            var binarySerializer = new DefaultBinarySerializer();

            var configuration = new RabbitMqConfiguration()
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "guest",
                UserPaasword = "guest",
                VirtualName = "/"
            };

            var producer = new MessageProducer(binarySerializer, configuration);

            var commandService = new CommandService(producer);

            var command = new CreateUserCommand(
                "天涯狼" + DateTime.Now.ToString("yyyyMMddHHmmsss"),
                DateTime.Now.ToString("yyyyMMddHHmmsss"),
                true,
                22);

            var consumer = new PushMessageConsumer(new RequestMessageContext()
            {
                Configuation = configuration,
                ExChangeName = typeof(CreateUserCommand).Namespace,
                ExchangeType = MessageExchangeType.Direct,
                NoAck = true,
                RoutingKey = MessageUtils.CurrentResponseRoutingKey,
                ShouldPersistent = false,
                ResponseRoutingKey = MessageUtils.CurrentResponseRoutingKey,
            }, new MessageResponseHandler());

            consumer.Start();

            Console.WriteLine("begin to receive the result message");

            for (var i = 0; i < 100; i++)
            {
                var commandResult = commandService.Send(command);

                Console.WriteLine("message:{0}", commandResult.Message);
            }
            Console.ReadLine();


        }
    }
}
