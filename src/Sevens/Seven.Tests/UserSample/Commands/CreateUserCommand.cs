using System;
using Seven.Commands;

namespace Seven.Tests.UserSample.Commands
{
    [Serializable]
    public class CreateUserCommand : Command
    {
        public string UserName { get; private set; }

        public string UserPassword { get; private set; }

        public bool Sex { get; private set; }

        public int Age { get; private set; }

        public CreateUserCommand(string userName, string userPassword, bool sex, int age) : base()
        {
            UserName = userName;
            UserPassword = userPassword;
            Sex = sex;
            Age = age;
        }

    }
}