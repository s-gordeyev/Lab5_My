using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;

namespace Lab5WorkerNode
{
    [Serializable]
    public abstract class Task
    {
        public int number { get; set; }
        public int lowerBound { get; set; }
        public int upperBound { get; set; }
        public int numberOfParall { get; set; }
        public const string connString = ".\\Private$\\RouterQueueAnswer";
        public int id;

        public Task(int lowerBound, int upperBound, int number)
        {
            this.number = number;
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
        }

        public Task() {
            number = lowerBound = upperBound = 0;
        }
        
        public abstract void run();

        protected void sendAnswer()
        {
            if (!MessageQueue.Exists(connString))
                MessageQueue.Create(connString);

            using (MessageQueue msQ = new MessageQueue(connString))
            {
                Message m = new Message(this);
                msQ.Send(m);
            }

        }
    }
}
