using System;
using Seven.Events;

namespace Seven.Tests.UserSample.DomainEvents
{
    [Serializable]
    public class BindEmailEvent : IDomainEvent
    {
        public string Email { get; private set; }

        public BindEmailEvent(string email)
        {
            this.Email = email;
        }
    }
}