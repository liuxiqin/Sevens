using System;
using Seven.Commands;

namespace Seven.Tests.UserSample.Commands
{
    [Serializable]
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
}