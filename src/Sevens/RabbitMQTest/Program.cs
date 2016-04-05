using System;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using Seven.Commands;
using Seven.Infrastructure.Serializer;
using Seven.Message;
using Seven.Tests.UserSample.Commands;

namespace RabbitMQServerTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var binarySerializer = new DefaultBinarySerializer();

            //var connectionInfo = new RabbitMqConnectionInfo("guest", "guest", "127.0.0.1", 5672);

            //var producer = new MessageProducer(binarySerializer);

            //var commandService = new CommandService(producer);

            //var comsumer = new MessageConsumer(binarySerializer, new DefaultJsonSerializer(), typeof(CreateUserCommand).FullName, "RPCRESPONSE");

            //while (true)
            //{
            //    var command = new CreateUserCommand(
            //        "天涯狼" + DateTime.Now.ToString("yyyyMMddHHmmsss"),
            //        DateTime.Now.ToString("yyyyMMddHHmmsss"),
            //        true,
            //        22);

            //    var commandHandleResult = commandService.Send(command);

            //    Console.WriteLine("comand result Message:{0} and status:{1}", commandHandleResult.Message,
            //        commandHandleResult.Status);

            //    Thread.Sleep(1000);
            //}
        }
    }
}
