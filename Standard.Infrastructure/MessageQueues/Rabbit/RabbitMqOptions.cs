using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.MessageQueues.Rabbit
{
    public class RabbitMqOptions
    {
        public const int DefaultConnectionTimeout = 30000;

        public const string DefaultPass = "guest";

        public const string DefaultUser = "guest";

        public const string DefaultVHost = "/";

        public const string DefaultExchangeName = "amq.direct";

        public const string ExchangeType = "topic";

        public bool AutomaticRecoveryEnabled
        {
            get;
            set;
        }

        public string HostName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public int QueueMessageExpires
        {
            get;
            set;
        }

        public int RequestedConnectionTimeout
        {
            get;
            set;
        }

        public int SocketReadTimeout
        {
            get;
            set;
        }

        public int SocketWriteTimeout
        {
            get;
            set;
        }

        public string TopicExchangeName
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string VirtualHost
        {
            get;
            set;
        }

        public RabbitMqOptions()
        {
            this.HostName = "localhost";
            this.Password = "guest";
            this.UserName = "guest";
            this.VirtualHost = "/";
            this.TopicExchangeName = "amq.direct";
            this.RequestedConnectionTimeout = 30000;
            this.SocketReadTimeout = 30000;
            this.SocketWriteTimeout = 30000;
            this.Port = -1;
            this.QueueMessageExpires = 864000000;
        }

        public void Valid()
        {
            if (String.IsNullOrEmpty(this.HostName))
            {
            }
        }
    }
}
