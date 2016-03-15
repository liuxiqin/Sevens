using Seven.Commands;
using Seven.Tests.UserSample.Commands;
using Seven.Tests.UserSample.Dmains;

namespace Seven.Tests.CommandHandlers
{
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