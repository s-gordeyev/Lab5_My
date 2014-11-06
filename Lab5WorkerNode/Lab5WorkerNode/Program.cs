using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Threading;

namespace Lab5WorkerNode
{
    class Program
    {
        const string connString = ".\\Private$\\RouterQueue";

        static void Main(string[] args)
        {

            while (true) {
                if (!MessageQueue.Exists(connString))
                {
                    Console.WriteLine("Queue is not found");
                    Thread.Sleep(1000);
                    continue;
                }

                using (MessageQueue msQ = new MessageQueue(connString))
                {
                    msQ.Formatter = new XmlMessageFormatter(new Type[] { typeof(TaskPrime) });
                    Message m = msQ.Receive();
                    TaskPrime str = (TaskPrime)m.Body;
                    str.run();
                    Console.WriteLine(str.answer + " " + str.id + " " + str.lowerBound + " " + str.upperBound + " " + str.number);
                }
            }
        }

        static void exec() {
        }
    }
}
