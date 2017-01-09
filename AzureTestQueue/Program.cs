// Course: https://www.udemy.com/70532-azure/learn/v4/overview
// Author course: Scott Duffy

using System;
using System.Globalization;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace AzureTestQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            var conn = System.Environment.GetEnvironmentVariable("QueueConnectionString", EnvironmentVariableTarget.User);
            var queue = "az532testqueue";

            if (conn == null)
            {
                Console.WriteLine("No define Environment Variable in system");
                return;
            }

            QueueClient client = QueueClient.CreateFromConnectionString(conn, queue);

            // S17L88
            // CreateMessage(client, 5);

            // S17L89
            // ReadAllMessage(client);


            // S17L90
            var topic = "az532testtopic";
            var topicclient = TopicClient.CreateFromConnectionString(conn, topic);
            var nsm = NamespaceManager.CreateFromConnectionString(conn);
            if (!nsm.SubscriptionExists(topic, "AllMessages"))
            {
                nsm.CreateSubscription(topic, "AllMessages");
            }
            CreateTopic(topicclient);

            // S17L91
            var subclient = SubscriptionClient.CreateFromConnectionString(conn, topic, "AllMessages");
            ReadTopic(subclient);
            

            Console.WriteLine("\nFinished.");
            Console.ReadKey();
        }

        private static void ReadTopic(SubscriptionClient client)
        {
            OnMessageOptions options = new OnMessageOptions();
            options.AutoComplete = false;

            client.OnMessage(message =>
            {
                Console.WriteLine("[Message] " + message.GetBody<string>());
                message.Complete();
            }, options);
        }

        private static void CreateTopic(TopicClient client)
        {
            var text = "" + DateTime.Now.ToString(CultureInfo.CurrentCulture) + " test topic.";
            var msg = new BrokeredMessage(text);
            client.Send(msg);
            Console.WriteLine("Topic send: " + text);
        }

        private static void ReadAllMessage(QueueClient client)
        {
            client.OnMessage(message =>
            {
                Console.WriteLine("[Message] " + message.GetBody<string>());
            });
        }

        private static void CreateMessage(QueueClient client, int iteration = 1)
        {
            for (int i = 0; i < iteration; i++)
            {
                var text = "" + DateTime.Now.ToString(CultureInfo.CurrentCulture) + " test message.";
                var msg = new BrokeredMessage(text);
                client.Send(msg);
                Console.WriteLine("Message send: " + text);
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
