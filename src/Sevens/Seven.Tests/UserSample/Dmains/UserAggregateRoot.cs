using System;
using Seven.Aggregates;
using Seven.Tests.UserSample.DomainEvents;

namespace Seven.Tests.UserSample.Dmains
{
    [Serializable]
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
}