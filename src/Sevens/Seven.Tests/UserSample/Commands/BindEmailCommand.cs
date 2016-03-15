using System;
using Seven.Commands;

namespace Seven.Tests.UserSample.Commands
{
    [Serializable]
    public class BindEmailCommand : Command
    {
        public string AggregateRootId { get; set; }

        public string Email { get; set; }

        public BindEmailCommand(string aggregateRootId, string email)
        {
            AggregateRootId = aggregateRootId;
            Email = email;
        }
    }
}