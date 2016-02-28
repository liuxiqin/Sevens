using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Seven.Aggregates;
using Seven.Commands;
using Seven.Events;

namespace CommandHandlerTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            ICommandBus bus = new CommandBus(Assembly.GetEntryAssembly());

            bus.Send(null);

        }
    }

    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public void Handle(ICommandContext commandContext, TestCommand command)
        {
            throw new NotImplementedException();
        }
    }

    public class TestCommand : ICommand
    {

        public void Execute()
        {
            Console.WriteLine("excute the command Execute");
        }

        public Guid CommandId
        {
            get { return new Guid(); }
        }
    }

    [Serializable]
    public class CreateUserCommand : Command
    {
        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public int Age { get; set; }

        public override string ToString()
        {
            return string.Format("UserName:{0},UserPassword:{1},Age:{2}", UserName, UserPassword, Age);
        }
    }

    public class UserCommandHandle : ICommandHandler<CreateUserCommand>
    {
        public void Handle(ICommandContext commandContext, CreateUserCommand command)
        {
            var objectId = Guid.NewGuid().ToString();

            commandContext.Add(new UserAggregateRoot(command.UserName, command.UserPassword, command.Age, objectId));
        }
    }

    public class CreateUserEvent : IDomainEvent
    {
        public string UserName { get; set; }

        public string AggregateRootId { get; set; }

        public string UserPassword { get; set; }

        public int Age { get; set; }

        public CreateUserEvent(string userName, string aggregateRootId,
            string userPassword, int age)
        {
            this.AggregateRootId = AggregateRootId;
            this.UserPassword = userPassword;
            this.UserName = userName;
            this.Age = age;
            this.AggregateRootId = aggregateRootId;
        }
    }


    public class UserAggregateRoot : AggregateRoot
    {
        public string UserName { get; private set; }

        public string UserPassword { get; private set; }

        public int Age { get; private set; }

        public UserAggregateRoot(
            string userName,
            string userPassword,
            int age,
            string aggregateRootId)
            : base(aggregateRootId)
        {
            ApplyEvent(new CreateUserEvent(userName, aggregateRootId, userPassword, age));
        }


        private void Handle(CreateUserEvent evnt)
        {
            this.UserName = evnt.UserName;
            this.UserPassword = evnt.UserPassword;
            this.AggregateRootId = AggregateRootId;
            this.Age = evnt.Age;
        }
    }
}
