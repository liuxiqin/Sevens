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
using System.Text;
using Seven.Infrastructure.IocContainer;

namespace RabbitMQServerTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var binarySerializer = new DefaultBinarySerializer();

            var command = new CreateUserCommand(
                "天涯狼" + DateTime.Now.ToString("yyyyMMddHHmmsss"),
                DateTime.Now.ToString("yyyyMMddHHmmsss"),
                true,
                22);

            var hostName = "127.0.0.1";
            var port = 5672;
            var userName = "guest";
            var password = "guest";
            var virtualName = "/";

            ObjectContainer.SetContainer(new AutofacContainerObject());

            var endPoint = new RemoteEndpoint(hostName, virtualName, userName, password, port);

            var exChangeName = typeof(CreateUserCommand).Namespace;

            var routingKey = MessageUtils.CurrentResponseRoutingKey;

            var responseRoutingKey = MessageUtils.CurrentResponseRoutingKey;

            var consumerContext = new ConsumerContext(exChangeName, responseRoutingKey, responseRoutingKey, routingKey, true);

            var channelPools = new CommunicateChannelFactoryPool(endPoint);

            var consumer = new PushMessageConsumer(channelPools, binarySerializer, consumerContext, new MessageResponseHandler());

            consumer.Start();

            ObjectContainer.RegisterInstance(channelPools);
            ObjectContainer.RegisterInterface<IBinarySerializer, DefaultBinarySerializer>();

            var requestChannelPools = new RequestChannelPools();

            var commandService = new CommandService(requestChannelPools);

            commandService.Send(command);

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
