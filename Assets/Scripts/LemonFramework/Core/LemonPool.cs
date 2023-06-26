using System.Collections.Generic;
using System;

namespace LemonFramework
{
    public sealed class LemonPool : IDisposable
    {
        private readonly Dictionary<Type, Queue<object>> pool = new Dictionary<Type, Queue<object>> ();

        public readonly static LemonPool Instance = new LemonPool ();

        private LemonPool ()
        {
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Fetch (Type type)
        {
            Queue<object> queue = null;
            if (!pool.TryGetValue (type, out queue))
            {
                return Activator.CreateInstance (type);
            }

            if (queue.Count == 0)
            {
                return Activator.CreateInstance (type);
            }
            return queue.Dequeue ();
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="obj"></param>
        public void Recycle (object obj)
        {
            Type type = obj.GetType ();
            Queue<object> queue = null;
            if (!pool.TryGetValue (type, out queue))
            {
                queue = new Queue<object> ();
                pool.Add (type, queue);
            }
            queue.Enqueue (obj);
        }

        public void Dispose ()
        {
            this.pool.Clear ();
        }
    }
}
