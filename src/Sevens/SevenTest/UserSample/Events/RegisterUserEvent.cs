using System;
using Seven.Events;

namespace SevenTest
{
    [Serializable]
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
}