using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Standard.Infrastructure.Helpers;
using Standard.Infrastructure.Iocs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.MessageQueues.Rabbit
{
    public class RabbitMqClent
    {
        private readonly IRabbitMqPool pool;

        public RabbitMqClent()
        {
            this.pool = new RabbitMqPool(this.CreateOptions());
        }

        public string CreateExchange(IModel channel, Action<RabbitMqClent.RabbitMqExchangeOptions> exchangeOptions)
        {
            string exchange;
            try
            {
                RabbitMqClent.RabbitMqExchangeOptions rabbitMqExchangeOption = new RabbitMqClent.RabbitMqExchangeOptions();
                exchangeOptions(rabbitMqExchangeOption);
                channel.ExchangeDeclare(rabbitMqExchangeOption.Exchange, rabbitMqExchangeOption.ExchangeType, rabbitMqExchangeOption.Durable, rabbitMqExchangeOption.AutoDelete, rabbitMqExchangeOption.Arguments);
                exchange = rabbitMqExchangeOption.Exchange;
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format("创建交换机失败,原因：{0}", exception.Message));
            }
            return exchange;
        }

        private RabbitMqOptions CreateOptions()
        {
            RabbitMqOptions rabbitMqOption = new RabbitMqOptions();
            IConfiguration service = Ioc.Container.GetService(typeof(IConfiguration)) as IConfiguration;
            service.GetSection("RabbitMq").Bind(rabbitMqOption);
            return rabbitMqOption;
        }

        public void SendDirectMsg(string exchange, string routeKey, object obj)
        {
            this.SendMsg(exchange, "direct", routeKey, obj);
        }

        public void SendFanoutMsg(string exchange, object obj)
        {
            this.SendMsg(exchange, "fanout", "", obj);
        }

        public void SendMsg(string exchange, string exchangeType, string routeKey, object obj)
        {
            this.SendMsg((RabbitMqClent.RabbitMqExchangeOptions options) => {
                options.Exchange = exchange;
                options.ExchangeType = exchangeType;
                options.Durable = true;
                options.AutoDelete = false;
            }, routeKey, obj);
        }

        public void SendMsg(Action<RabbitMqClent.RabbitMqExchangeOptions> exchangeOptions, string routeKey, object obj)
        {
            IModel model = this.pool.Rent();
            try
            {
                try
                {
                    string str = this.CreateExchange(model, exchangeOptions);
                    IBasicProperties basicProperty = model.CreateBasicProperties();
                    basicProperty.Persistent = true;
                    string json = JsonHelper.ToJson(obj);
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    model.BasicPublish(str, routeKey, basicProperty, bytes);
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    //Log.GetLog("NLog").Error(String.Concat("RabbitMq消息推送失败,原因：", exception.Message));
                }
            }
            finally
            {
                this.pool.Return(model);
            }
        }

        public void SendTopicMsg(string exchange, string routeKey, object obj)
        {
            this.SendMsg(exchange, "topic", routeKey, obj);
        }

        public class RabbitMqExchangeOptions
        {
            public IDictionary<string, object> Arguments
            {
                get;
                set;
            }

            public bool AutoDelete
            {
                get;
                set;
            }

            public bool Durable
            {
                get;
                set;
            }

            public string Exchange
            {
                get;
                set;
            }

            public string ExchangeType
            {
                get;
                set;
            }

            public RabbitMqExchangeOptions()
            {
                this.Durable = false;
                this.AutoDelete = false;
                this.Arguments = null;
            }
        }
    }
}
