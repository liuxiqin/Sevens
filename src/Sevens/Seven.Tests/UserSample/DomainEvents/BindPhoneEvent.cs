using System;
using Seven.Events;

namespace Seven.Tests.UserSample.DomainEvents
{
    [Serializable]
    public class BindPhoneEvent : IDomainEvent
    {
        public string Phone { get; private set; }

        public BindPhoneEvent(string phone)
        {
            this.Phone = phone;
        }
    }
}