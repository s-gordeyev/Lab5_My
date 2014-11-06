using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Threading;

namespace Lab5MainNode
{
    public class TaskManager
    {
        static List<Task> tasks = new List<Task>();
        static int counter = 0;
        static List<Thread> trds = new List<Thread>();

        static TaskManager()
        {
            AddChecking(TaskManager.CheckAnswerPrime);
        }

        static void AddChecking(Action a){
            Thread t = new Thread(new ThreadStart(a));
            t.Start();
            trds.Add(t);
        }

        static public void dispose()
        {
            trds.ForEach(delegate(Thread t) {
                try
                {
                    t.Abort();
                }
                catch {}
            });
        }

        static void add(Task t) 
        {
            tasks.Add(t);
            Program.send(t, Program.connString);
        }

        public static int NewTaskPrime(int number, int numberOfParall) 
        {
            if (numberOfParall < 1)
                numberOfParall = 1;

            TaskManager.counter++;

            int nsq = (int)Math.Sqrt(number) + 1;
            int prev = 2;

            int maxNumberOfParall = Math.Max(nsq / 4, 1);

            if (maxNumberOfParall < numberOfParall)
                numberOfParall = maxNumberOfParall;
            
            for (int i = 0; i < numberOfParall; i++)
            {
                int cur = nsq * (i + 1) / numberOfParall;
                TaskPrime tp = new TaskPrime { lowerBound = prev, upperBound = cur,
                                              number = number, numberOfParall = numberOfParall, id = TaskManager.counter};
                TaskManager.add(tp);
                prev = cur + 1;
            }

            return TaskManager.counter;
        }

        static void CheckAnswerPrime() 
        {
            using (MessageQueue mq = new MessageQueue(Program.connStringAns)) 
            {
                while (true)
                {
                    if (tasks.Count == 0)
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(TaskPrime) });
                    TaskPrime tp = (TaskPrime)mq.Receive().Body;
                    TaskPrime hk = (TaskPrime)tasks.FirstOrDefault<Task>(task => (task.id == tp.id && task.lowerBound == tp.lowerBound));
                    hk.answer = tp.answer;
                }
            }
        }

        public static short GetAnswerByIdPrime(int id) 
        {
            return TaskPrime.group(tasks.Where<Task>(x => x.id == id).Select(t => (TaskPrime)t).ToArray<TaskPrime>());
        }

    }
}
