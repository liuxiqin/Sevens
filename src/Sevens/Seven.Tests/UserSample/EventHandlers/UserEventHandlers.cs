using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Seven.Events;
using Seven.Tests.UserSample.DomainEvents;

namespace Seven.Tests.UserSample.EventHandlers
{
    public class UserEventHandlers
        :IEventHandler<RegisterUserEvent>,
        IEventHandler<BindEmailEvent>,
        IEventHandler<BindPhoneEvent>,
        IEventHandler<ChangePasswordEvent>
    {
        public void Handle(RegisterUserEvent evnt)
        {
            throw new NotImplementedException();
        }

        public void Handle(BindEmailEvent evnt)
        {
            throw new NotImplementedException();
        }

        public void Handle(ChangePasswordEvent evnt)
        {
            throw new NotImplementedException();
        }

        public void Handle(BindPhoneEvent evnt)
        {
            throw new NotImplementedException();
        }
    }
}
