using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Threading;
using System.Numerics;

namespace Lab5MainNode
{
    class Program
    {
        public const string connString = ".\\Private$\\RouterQueue";
        public const string connStringAns = ".\\Private$\\RouterQueueAnswer";

        static void Main(string[] args)
        {
            createQueues();

            int id = TaskManager.NewTaskPrime(4, 2);
            short ans = -1;
            while ((ans = TaskManager.GetAnswerByIdPrime(id)) == -1) 
                Thread.Sleep(10);

            Console.WriteLine(ans);

            TaskManager.dispose();
        }

        public static void send(Object obj, string connString){
            using (MessageQueue msQ = new MessageQueue(connString))
            {
                Message m = new Message(obj);
                msQ.Send(m);
            }
        }

        public static void createQueues()
        {
            if (!MessageQueue.Exists(connString))
                MessageQueue.Create(connString);

            if (!MessageQueue.Exists(connStringAns))
                MessageQueue.Create(connStringAns);
        }
    }
}
