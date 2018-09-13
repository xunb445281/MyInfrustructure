using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Standard.Infrastructure.MessageQueues.Rabbit
{
    public class RabbitMqPool : IRabbitMqPool
    {
        private const int defaultPoolSize = 50;

        private readonly Func<IConnection> connectionActivator;

        private readonly ConcurrentQueue<IModel> pool = new ConcurrentQueue<IModel>();

        private IConnection connection;

        private int count;

        private int maxSize;

        public RabbitMqPool(RabbitMqOptions options)
        {
            this.maxSize = 50;
            this.connectionActivator = RabbitMqPool.CreateConnection(options);
        }

        private static Func<IConnection> CreateConnection(RabbitMqOptions options)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = options.HostName,
                UserName = options.UserName,
                Password = options.Password,
                Port = options.Port,
                VirtualHost = options.VirtualHost,
                RequestedConnectionTimeout = options.RequestedConnectionTimeout,
                SocketReadTimeout = options.SocketReadTimeout,
                SocketWriteTimeout = options.SocketWriteTimeout,
                AutomaticRecoveryEnabled = options.AutomaticRecoveryEnabled
            };
            Func<IConnection> func = () => connectionFactory.CreateConnection();
            return func;
        }

        public void Dispose()
        {
            IModel model;
            this.maxSize = 0;
            while (this.pool.TryDequeue(out model))
            {
                model.Dispose();
            }
        }

        public IConnection GetConnection()
        {
            IConnection connection;
            if ((this.connection == null ? true : !this.connection.IsOpen))
            {
                this.connection = this.connectionActivator();
                this.connection.ConnectionShutdown += new EventHandler<ShutdownEventArgs>(this.RabbitMq_ConnectionShutdown);
                connection = this.connection;
            }
            else
            {
                connection = this.connection;
            }
            return connection;
        }

        private void RabbitMq_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public IModel Rent()
        {
            if (!this.pool.TryDequeue(out IModel model))
            {
                model = this.GetConnection().CreateModel();
            }
            else
            {
                Interlocked.Decrement(ref this.count);
            }
            return model;
        }

        public bool Return(IModel model)
        {
            if (Interlocked.Increment(ref this.count) > this.maxSize)
            {
                Interlocked.Decrement(ref this.count);
                return false;
            }
            else
            {
                this.pool.Enqueue(model);
            }
            return true;
        }
    }
}
