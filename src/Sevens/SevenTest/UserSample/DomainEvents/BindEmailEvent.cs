using System;
using Seven.Events;

namespace SevenTest
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