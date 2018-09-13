using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.MessageQueues.Rabbit
{
    public interface IRabbitMqPool
    {
        IConnection GetConnection();

        IModel Rent();

        bool Return(IModel model);
    }
}
