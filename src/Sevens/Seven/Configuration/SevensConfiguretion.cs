using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Messages;

namespace Seven.Configuration
{
    public static class SevensConfiguretion
    {
        public static RabbitMqConfiguration RabbitMqConfiguration { get; private set; }

        public static void SetRabbitMqConfiguration(RabbitMqConfiguration configuration)
        {
            RabbitMqConfiguration = configuration;
        }

    }
}
