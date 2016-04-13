﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace Seven.Messages
{
    /// <summary>
    /// 消息Broker
    /// </summary>
    public interface IMessageBroker
    {
        void Stop();

        IConnection GetConnection { get; }

    }
}