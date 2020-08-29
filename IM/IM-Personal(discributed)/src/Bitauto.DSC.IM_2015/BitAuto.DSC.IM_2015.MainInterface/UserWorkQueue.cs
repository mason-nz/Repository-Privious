using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;

namespace BitAuto.DSC.IM_2015.MainInterface
{

    /// <summary>
    /// Represents an queue allowing asynchronous user work
    /// </summary>
    /// <typeparam name="T">Type of element in the queue</typeparam>
    public class UserWorkQueue<T>
    {
        /// <summary>
        /// Underlying <see cref="Queue"/> object
        /// </summary>
        private ConcurrentQueue<T> queue;

        /// <summary>
        /// The synchronization object for queue
        /// </summary>
        //private object lockObj = new object();

        /// <summary>
        /// Indicate whether the user work is inprogress
        /// </summary>
        //private bool isWorking;
        private int _isWorking = 0;

        /// <summary>
        /// The synchronization object for isWorking
        /// </summary>
        //private object lockIsWorking = new object();

        /// <summary>
        /// Get the value whether the user work is inprogress
        /// </summary>
        //public bool IsWorking
        //{
        //    get { return _isWorking; }
        //}

        /// <summary>
        /// The event that is fired when an element in queue is dequeued.
        /// </summary>
        public event EventHandler<EnqueueEventArgs<T>> DoUserWork;

        /// <summary>
        /// Initialize an instance of <see cref="UserWorkQueue"/> class that is empty
        /// and has the default initial capacity
        /// </summary>
        public UserWorkQueue()
        {
            queue = new ConcurrentQueue<T>();
        }


        private bool isSequential;

        /// <summary>
        /// Determines whether queue object's porcess is using single thread 
        /// ture means that porcess is using single thread.
        /// </summary>
        public bool ISSequential
        {
            get
            {
                return isSequential;
            }
            set
            {
                isSequential = value;
            }
        }

        /// <summary>
        /// Add the object to the queue. 
        /// If a work event handle is attached, it will trigger the object's process.
        /// </summary>
        /// <param name="item"></param>
        public void EnqueueToProcess(T item)
        {
            queue.Enqueue(item);

            if (Interlocked.CompareExchange(ref _isWorking, 1, 0) == 0)
            {
                ThreadPool.QueueUserWorkItem(doUserWork);
            }

            //lock (lockIsWorking)
            //{
            //    if (!_isWorking)
            //    {
            //        _isWorking = true;
            //        ThreadPool.QueueUserWorkItem(doUserWork);
            //    }
            //}
        }

        /// <summary>
        /// Process the object at the beginning of the queue,
        /// then remove it from the queue, repeat untill all elements were processed.
        /// </summary>
        /// <param name="obj">user state, it is not used here because 
        /// we can obtain the element by calling Dequeue</param>
        private void doUserWork(object obj)
        {
            try
            {
                T item;
                while (true)
                {
                    //lock (lockObj)
                    //{
                    //    if (queue.Count > 0)
                    //    {
                    //        item = queue.Dequeue();
                    //    }
                    //    else
                    //    {
                    //        return;
                    //    }
                    //}
                    if (!queue.TryDequeue(out item))
                    {
                        return;
                    }

                    if (!item.Equals(default(T)))
                    {
                        if (isSequential)
                        {
                            if (DoUserWork != null)
                            {
                                DoUserWork(this, new EnqueueEventArgs<T>(item));
                            }
                        }
                        else
                        {
                            ThreadPool.QueueUserWorkItem(o =>
                            {
                                if (DoUserWork != null)
                                {
                                    DoUserWork(this, new EnqueueEventArgs<T>(o));
                                }
                            }, item);
                        }
                    }
                }
            }
            finally
            {
                //lock (lockIsWorking)
                //{
                //    _isWorking = false;
                //}
                Interlocked.Exchange(ref _isWorking, 0);
            }
        }
    }

    /// <summary>
    /// Event data sent within firing <see cref="DoUserWork"/> event
    /// </summary>
    public class EnqueueEventArgs<T> : EventArgs
    {
        public T Item { get; private set; }
        public EnqueueEventArgs(object item)
        {
            try
            {
                Item = (T)item;
            }
            catch (Exception)
            {
                throw new InvalidCastException("Convert object to T failed!");
            }
        }
    }
}
