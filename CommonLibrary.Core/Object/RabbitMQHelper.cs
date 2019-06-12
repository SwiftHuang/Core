using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;
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
        public static void PublishMsg<T>(RabbitMQSetting mqSetting, QueueSetting qSetting, T msg, bool persistent = true)
        {
            BasicProperties bp = new BasicProperties()
            {
                //队列持久化
                Persistent = persistent
            };

            PublishMsg<T>(mqSetting, qSetting, bp, msg);
        }
        public static void PublishMsgForEasyNetQ<T>(RabbitMQSetting mqSetting, QueueSetting qSetting, T msg, bool persistent = true)
        {
            BasicProperties bp = new BasicProperties()
            {
                //队列持久化
                Persistent = persistent,
                Type = RemoveAssemblyDetails(typeof(T).AssemblyQualifiedName)
            };
            PublishMsg<T>(mqSetting, qSetting, bp, msg);
        }
        public static void PublishMsg<T>(RabbitMQSetting mqSetting, QueueSetting qSetting, BasicProperties basicProperties, T msg)
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
                    basicProperties: basicProperties,
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
                    Data = new DataInfo<T>(),
                };

                try
                {
                    var msg = Encoding.UTF8.GetString(ea.Body);
                    if (typeof(T).Name != "String")
                    {
                        args.Data.Value = JsonConvert.DeserializeObject<T>(msg);
                    }
                    else
                    {
                        args.Data.Value = msg as T;
                    }
                }
                catch (Exception ex)
                {
                    args.Data.DeserializeSuccess = false;
                    args.Data.ExceptionInfo = ex;
                }
                action(args);
            };
            string consumerTag = channel.BasicConsume(qSetting.QueueName, false, c);
        }

        private static string RemoveAssemblyDetails(string fullyQualifiedTypeName)
        {
            var builder = new StringBuilder(fullyQualifiedTypeName.Length);

            // loop through the type name and filter out qualified assembly details from nested type names
            var writingAssemblyName = false;
            var skippingAssemblyDetails = false;
            foreach (var character in fullyQualifiedTypeName)
            {
                switch (character)
                {
                    case '[':
                        writingAssemblyName = false;
                        skippingAssemblyDetails = false;
                        builder.Append(character);
                        break;
                    case ']':
                        writingAssemblyName = false;
                        skippingAssemblyDetails = false;
                        builder.Append(character);
                        break;
                    case ',':
                        if (!writingAssemblyName)
                        {
                            writingAssemblyName = true;
                            builder.Append(character);
                        }
                        else
                        {
                            skippingAssemblyDetails = true;
                        }
                        break;
                    default:
                        if (!skippingAssemblyDetails)
                        {
                            builder.Append(character);
                        }
                        break;
                }
            }

            return builder.ToString();
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

            public RabbitMQSetting(string connectionString)
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    string[] connectionStringSplit = connectionString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var conSettingStr in connectionStringSplit)
                    {
                        string[] conSettingStrSplit = conSettingStr.Split('=');
                        if (conSettingStrSplit.Length > 1)
                        {
                            string settingType = conSettingStrSplit[0].Trim();
                            string settingValue = string.Empty;
                            for (int i = 1; i < conSettingStrSplit.Length; i++)
                            {
                                settingValue += conSettingStrSplit[i] + '=';
                            }
                            settingValue = settingValue.Remove(settingValue.Length - 1).Trim();

                            if (settingType.Equals("host", StringComparison.OrdinalIgnoreCase))
                            {
                                HostName = settingValue;
                            }
                            else if (settingType.Equals("virtualHost", StringComparison.OrdinalIgnoreCase))
                            {
                                VirtualHost = settingValue;
                            }
                            else if (settingType.Equals("username", StringComparison.OrdinalIgnoreCase))
                            {
                                UserName = settingValue;
                            }
                            else if (settingType.Equals("password", StringComparison.OrdinalIgnoreCase))
                            {
                                Password = settingValue;
                            }
                        }
                    }
                }
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
            public DataInfo<T> Data { get; set; }
        }
        public class DataInfo<T> where T : class
        {
            public T Value { get; set; }
            public bool DeserializeSuccess { get; set; }
            public Exception ExceptionInfo { get; set; }

            public DataInfo()
            {
                DeserializeSuccess = true;
            }
        }
    }
}