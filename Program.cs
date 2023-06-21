using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace DefaultDemo
{
    class Program
    {
        //This is an example for the "Default Exchange":
        //1. When a new queue is created on RabbitMQ system, it is implicitly bound
        //to a system exchange called "default exchange", with a routing key which is
        //the same as the queue name.
        //2. Default echange has no name (empty string).
        //3. The type of default exchange is "direct".
        //4. When sending a message, if exchange name is left empty, it is habdled by the "default exchange"

        static void Main(string[] args)
        {
            IConnection conn;
            IModel channel;

            ConnectionFactory factory = new ConnectionFactory();
            // "guest"/"guest" by default, limited to localhost connections
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";

            conn = factory.CreateConnection();
            channel = conn.CreateModel();

            //Enable channel level publisher confirms (to be able to get confirmations from RabbitMQ broker)
            channel.ConfirmSelect();

            channel.QueueDeclare(
                "my.queue1",
                true,
                false,
                false,
                null);

            channel.QueueDeclare(
                "my.queue2",
                true,
                false,
                false,
                null);

            channel.BasicPublish(
                "",
                "my.queue1",
                null,
                Encoding.UTF8.GetBytes("Message with routing key my.queue1"));

            channel.BasicPublish(
                "",
                "my.queue2",
                null,
                Encoding.UTF8.GetBytes("Message with routing key my.queue2"));

            //Wait until all published messages are confirmed
            channel.WaitForConfirms();
        }
    }
}
