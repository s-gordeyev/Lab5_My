using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;


namespace Lab5MainNode
{
    [Serializable]
    public class TaskPrime:Task
    {
        

        public TaskPrime(int lowerBound, int upperBound, int number) : base(lowerBound, upperBound, number)
        {
            this.answer = -1;
            this.type = "TaskPrime";
        }

        public TaskPrime() : base() 
        {
            this.answer = -1;
            this.type = "TaskPrime";
        }

        // -1 - is not processed, 0 - is not prime, 1 - is prime
        public short answer { get; set; }

        override public void run() 
        {
            this.answer = 1;

            for (int i = this.lowerBound; i < this.upperBound; i++)
            {
                if (this.number == i)
                    break;

                if (this.number % i == 0)
                {
                    this.answer = 0;
                    break;
                }
            }

            this.sendAnswer();
        }

        static public object group(TaskPrime[] tp)
        {
            if (tp.Where<TaskPrime>(x => x.answer != -1).Count<TaskPrime>() < tp[0].numberOfParall)
                return -1;

            return (short)((tp.Aggregate(0, (total, x) => total + x.answer) == tp[0].numberOfParall) ? 1 : 0);
        }
    }
}
