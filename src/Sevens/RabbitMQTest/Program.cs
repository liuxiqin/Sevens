using System;
using System.Diagnostics;
using Seven.Commands;
using Seven.Infrastructure.Serializer;
using Seven.Messages;
using Seven.Messages.Channels;
using Seven.Messages.QueueMessages;
using Seven.Tests.UserSample.Commands;
using Seven.Infrastructure.Dependency;
using Seven.Tests;

namespace RabbitMQServerTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var binarySerializer = new DefaultBinarySerializer();

            var hostName = "127.0.0.1";
            var port = 5672;
            var userName = "guest";
            var password = "guest";
            var virtualName = "/";

            ObjectContainer.SetContainer(new AutofacContainerObject());

            var endPoint = new RemoteEndpoint(hostName, virtualName, userName, password, port);

            var exChangeName = typeof(CreateUserCommand).Assembly.GetName().Name;

            var routingKey = MessageUtils.CurrentResponseRoutingKey;

            var responseRoutingKey = MessageUtils.CurrentResponseRoutingKey;

            var consumerContext = new ConsumerContext(exChangeName, responseRoutingKey, responseRoutingKey, routingKey, true);

            var channelPools = new CommunicateChannelFactoryPool(endPoint);

            var consumer = new PushMessageConsumer(channelPools, binarySerializer, consumerContext, new MessageResponseHandler());

            consumer.Start();


            ObjectContainer.RegisterInstance(channelPools);
            ObjectContainer.RegisterInterface<IBinarySerializer, DefaultBinarySerializer>();

            var requestChannelPools = new RequestChannelPools();

            var commandTopicProvider = new UserTopicProvider();

            var commandService = new CommandService(requestChannelPools, commandTopicProvider);

            Console.WriteLine("begin to receive the result message");

            Stopwatch watch = new Stopwatch();
            watch.Start();


            for (var i = 0; i < 10000; i++)
            {
                var command = new CreateUserCommand(
                 "天涯狼" + DateTime.Now.ToString("yyyyMMddHHmmsss"),
                 DateTime.Now.ToString("yyyyMMddHHmmsss"),
                 true,
                 22);

                var commandResult = commandService.Send(command, 20);

                Console.WriteLine("message:{0} and number is {1}", commandResult.Message, i);
            }

            watch.Stop();

            Console.WriteLine("message:{0} ", watch.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}
