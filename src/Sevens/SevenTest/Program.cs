using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Seven.Aggregates;
using Seven.Commands;
using Seven.Events;
using Seven.Infrastructure.Ioc;
using Seven.Infrastructure.Repository;
using Seven.Initializer;

namespace SevenTest
{
    public class Program
    {
        private static void Main(string[] args)
        {

            ObjectContainer.SetContainer(new AutofacContainerObject());

            var applictionInitializer = new EventHandleProvider();

            applictionInitializer.Initialize(Assembly.GetExecutingAssembly());

            var commandInitializer = new CommandHandleProvider();

            commandInitializer.Initialize(Assembly.GetExecutingAssembly());

            ObjectContainer.RegisterInstance(applictionInitializer);
            ObjectContainer.RegisterInstance(commandInitializer);

            IRepository repository = new EventSouringRepository(null, null);

            var comamndHandler = ObjectContainer.Resolve<CommandHandleProvider>();

            var createUserCommand = new CreateUserCommand("张杰", "2222222", false, 18);

            var commandContext = new CommandContext(repository);

            var commandHanldeAction = comamndHandler.GetInternalCommandHandle(typeof(CreateUserCommand));
            commandHanldeAction(commandContext, createUserCommand);

            var aggregateRoots = commandContext.AggregateRoots;

            foreach (var aggregateRoot in aggregateRoots)
            {
                var unCommitEvents = aggregateRoot.Value.Commit();
            }

            Console.WriteLine("改方法执行完毕...");

        }
    }

    public class UserAggregateRoot : AggregateRoot
    {
        public string UserName { get; private set; }

        public string UserPassword { get; private set; }

        public bool Sex { get; private set; }

        public int Age { get; private set; }

        public string Phone { get; private set; }

        public string Email { get; private set; }

        public UserAggregateRoot(string userName, string userPassword, bool sex, int age)
            : base(Guid.NewGuid().ToString())
        {
            ApplyEvent(new RegisterUserEvent(userName, userPassword, sex, age));
        }

        public void BindEmail(string email)
        {
            ApplyEvent(new BindEmailEvent(email));
        }



        public void BindPhone(string phone)
        {
            ApplyEvent(new BindPhoneEvent(phone));

        }

        private void Handle(RegisterUserEvent evnt)
        {
            this.UserName = evnt.UserName;
            this.UserPassword = evnt.UserPassword;
            this.Sex = evnt.Sex;
            this.Age = evnt.Age;
        }

        private void Handle(BindEmailEvent evnt)
        {
            this.Email = evnt.Email;
        }
        public void ChangePassword(string oldPassword, string newPassword)
        {
            ApplyEvent(new ChangePasswordEvent(oldPassword, newPassword));
        }

        private void Handle(ChangePasswordEvent evnt)
        {
            if (!UserPassword.Equals(evnt.OldPassword))
            {
                throw new ApplicationException("the old password is not true");
            }
            this.UserPassword = evnt.NewPassword;
        }

        private void Hanlde(BindPhoneEvent evnt)
        {
            this.Phone = evnt.Phone;
        }
    }

    public class RegisterUserEvent : IDomainEvent
    {
        public string UserName { get; private set; }

        public string UserPassword { get; private set; }

        public bool Sex { get; private set; }

        public int Age { get; private set; }

        public RegisterUserEvent(
            string userName,
            string userPasword,
            bool sex,
            int age)
        {
            this.UserName = userName;
            this.UserPassword = userPasword;
            this.Sex = sex;
            this.Age = age;
        }
    }

    public class BindEmailEvent : IDomainEvent
    {
        public string Email { get; private set; }

        public BindEmailEvent(string email)
        {
            this.Email = email;
        }
    }

    public class BindPhoneEvent : IDomainEvent
    {
        public string Phone { get; private set; }

        public BindPhoneEvent(string phone)
        {
            this.Phone = phone;
        }
    }

    public class ChangePasswordEvent : IDomainEvent
    {
        public string OldPassword { get; private set; }

        public string NewPassword { get; private set; }

        public ChangePasswordEvent(string oldPassword, string newPassword)
        {
            this.OldPassword = oldPassword;
            this.NewPassword = newPassword;
        }
    }

    public class CreateUserCommand : Command
    {
        public string UserName { get; private set; }

        public string UserPassword { get; private set; }

        public bool Sex { get; private set; }

        public int Age { get; private set; }

        public CreateUserCommand(string userName, string userPassword, bool sex, int age)
        {
            UserName = userName;
            UserPassword = userPassword;
            Sex = sex;
            Age = age;
        }

    }

    public class ChangePasswordCommand : Command
    {
        public string AggregateRootId { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public ChangePasswordCommand(string aggregateRootId, string oldPassword, string newPassword)
        {
            AggregateRootId = aggregateRootId;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }

    public class BindEmailCommand : Command
    {
        public string AggregateRootId { get; set; }

        public string Email { get; set; }

        public BindEmailCommand(string aggregateRootId, string email)
        {
            AggregateRootId = aggregateRootId;
            Email = email;
        }
    }
    public class UserCommandHandler :
        ICommandHandler<CreateUserCommand>,
        ICommandHandler<ChangePasswordCommand>,
        ICommandHandler<BindEmailCommand>
    {
        public void Handle(ICommandContext commandContext, CreateUserCommand command)
        {
            commandContext.Add(new UserAggregateRoot(command.UserName, command.UserPassword, command.Sex, command.Age));
        }

        public void Handle(ICommandContext commandContext, ChangePasswordCommand command)
        {
            commandContext.Get<UserAggregateRoot>(command.AggregateRootId).ChangePassword(command.OldPassword, command.NewPassword);
        }

        public void Handle(ICommandContext commandContext, BindEmailCommand command)
        {
            commandContext.Get<UserAggregateRoot>(command.AggregateRootId).BindEmail(command.Email);
        }
    }
}