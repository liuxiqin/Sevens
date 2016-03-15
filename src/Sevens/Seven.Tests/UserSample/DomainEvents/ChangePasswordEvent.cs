using System;
using Seven.Events;

namespace Seven.Tests.UserSample.DomainEvents
{
    [Serializable]
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
}