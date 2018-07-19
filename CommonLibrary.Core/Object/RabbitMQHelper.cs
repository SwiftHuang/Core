using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace hwj.CommonLibrary.Core.Object
{
    public class RabbitMQHelper
    {
        private static ConnectionFactory InitMQConn(RabbitMQSetting setting)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = setting.UserName;
            factory.Password = setting.Password;
            factory.VirtualHost = setting.VirtualHost;
            factory.HostName = setting.HostName;

            return factory;
        }

        public static void PublishMsg(RabbitMQSetting mqSetting, QueueSetting qSetting, object msg)
        {
            ConnectionFactory factory = InitMQConn(mqSetting);

            using (IConnection conn = factory.CreateConnection())
            using (IModel channel = conn.CreateModel())
            {
                channel.ExchangeDeclare(
                    exchange: qSetting.ExchangeName,
                    type: qSetting.ExchangeType,
                    durable: qSetting.ExchangeDurable);
                channel.QueueDeclare(
                    queue: qSetting.QueueName,
                    durable: qSetting.QueueDurable,
                    exclusive: false,
                    autoDelete: false);
                channel.QueueBind(
                    queue: qSetting.QueueName,
                    exchange: qSetting.ExchangeName,
                    routingKey: qSetting.RoutingKey);

                byte[] data;
                if (msg.GetType().Name == "String")
                {
                    data = Encoding.UTF8.GetBytes(msg.ToString());
                }
                else
                {
                    string tmpJson = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
                    data = Encoding.UTF8.GetBytes(tmpJson);
                }

                channel.BasicPublish(
                    exchange: qSetting.ExchangeName,
                    routingKey: qSetting.RoutingKey,
                    basicProperties: null,
                    body: data);
            }
        }

        public static void ReceiveMsg<T>(RabbitMQSetting mqSetting, QueueSetting qSetting, Action<ActionArgs<T>> action) where T : class
        {
            ConnectionFactory factory = InitMQConn(mqSetting);
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.ExchangeDeclare(
                  exchange: qSetting.ExchangeName,
                  type: qSetting.ExchangeType,
                  durable: qSetting.ExchangeDurable);

            channel.QueueDeclare(
                queue: qSetting.QueueName,
                durable: qSetting.QueueDurable,
                exclusive: false,
                autoDelete: false);

            channel.QueueBind(
                queue: qSetting.QueueName,
                exchange: qSetting.ExchangeName,
                routingKey: qSetting.RoutingKey);

            EventingBasicConsumer c = new EventingBasicConsumer(channel);
            c.Received += (ch, ea) =>
            {
                ActionArgs<T> args = new ActionArgs<T>()
                {
                    Channel = channel,
                    EventArgs = ea,
                };
                try
                {
                    var msg = Encoding.UTF8.GetString(ea.Body);
                    if (typeof(T).Name != "String")
                    {
                        args.Data = JsonConvert.DeserializeObject<T>(msg);
                    }
                    else
                    {
                        args.Data = msg as T;
                    }
                }
                catch (Exception ex)
                {

                }
                action(args);
            };
            string consumerTag = channel.BasicConsume(qSetting.QueueName, false, c);
        }

        public class RabbitMQSetting
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string VirtualHost { get; set; }
            public string HostName { get; set; }

            public RabbitMQSetting()
            {
            }
        }

        public class QueueSetting
        {
            public string ExchangeName { get; set; }
            public string ExchangeType { get; set; }
            public bool ExchangeDurable { get; set; }
            public string QueueName { get; set; }
            public bool QueueDurable { get; set; }
            public string RoutingKey { get; set; }

            public QueueSetting()
            {
            }
        }

        public class ActionArgs<T> where T : class
        {
            public IModel Channel { get; set; }
            public BasicDeliverEventArgs EventArgs { get; set; }
            public T Data { get; set; }
        }
    }
}