using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beethoven3
{
    public class MyQueue<T>
    {

        Queue<T> m_Queue = new Queue<T>();

        public Queue<T> oQueue
        {

            get { return m_Queue; }

            set { m_Queue = value; }

        }

        int iFixedCount = 100;

        T iLastValue;



        public MyQueue(int _count)
        {

            iFixedCount = _count;

        }



        public Int32 Count
        {

            get { return m_Queue.Count; }

        }



        public void Enqueue(T oItem)
        {

            iLastValue = oItem;

            m_Queue.Enqueue(oItem);

            if (m_Queue.Count > iFixedCount)

                m_Queue.Dequeue();

        }





        public T LastValue
        {

            get { return iLastValue; }

        }

    }

 
}
