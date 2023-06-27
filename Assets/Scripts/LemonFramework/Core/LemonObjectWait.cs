using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Diagnostics;

namespace LemonFramework
{
    public class LemonObjectWait
    {
        public interface IDestroyRun
        {
            void SetResult ();
        }

        public class ResultCallback<K> : IDestroyRun where K : struct, IWaitType
        {
            private UniTaskCompletionSource<K> tcs;

            public ResultCallback ()
            {
                this.tcs = new UniTaskCompletionSource<K> ();
            }

            public bool IsDisposed
            {
                get
                {
                    return this.tcs == null;
                }
            }

            public UniTaskCompletionSource<K> Task => this.tcs;

            public void SetResult (K k)
            {
                var t = tcs;
                this.tcs = null;
                t.TrySetResult (k);
            }

            public void SetResult ()
            {
                var t = tcs;
                this.tcs = null;
                t.TrySetResult (new K () { State = LemonWaitType.Destroy });
            }
        }


        private readonly Dictionary<Type, object> tcss = new ();

        public async UniTask<T> Wait<T> () where T : struct, IWaitType
        {
            ResultCallback<T> tcs = new ResultCallback<T> ();
            Type type = typeof (T);
            this.tcss.Add (type, tcs);
            T ret = await tcs.Task.Task;
            return ret;
        }

        public async UniTask<T> Wait<T> (T a) where T : struct, IWaitType
        {
            ResultCallback<T> tcs = new ResultCallback<T> ();
            Type type = typeof (T);
            this.tcss.Add (type, tcs);
            T ret = await tcs.Task.Task;
            return ret;
        }

        public async UniTask<T> Wait<T> (CancellationToken cancellationToken) where T : struct, IWaitType
        {
            ResultCallback<T> tcs = new ResultCallback<T> ();
            Type type = typeof (T);
            this.tcss.Add (type, tcs);

            void CancelAction ()
            {
                this.Notify (new T () { State = LemonWaitType.Cancel });
            }

            T ret;
            try
            {
                cancellationToken.Register (CancelAction);
                ret = await tcs.Task.Task;
            }
            finally
            {

            }
            return ret;
        }

        public async UniTask<T> Wait<T> (T a, CancellationToken cancellationToken) where T : struct, IWaitType
        {
            ResultCallback<T> tcs = new ResultCallback<T> ();
            Type type = typeof (T);
            this.tcss.Add (type, tcs);

            void CancelAction ()
            {
                this.Notify (new T () { State = LemonWaitType.Cancel });
            }

            T ret;
            try
            {
                cancellationToken.Register (CancelAction);
                ret = await tcs.Task.Task;
            }
            finally
            {

            }
            return ret;
        }

        public async UniTask<T> Wait<T> (int timeout) where T : struct, IWaitType
        {
            ResultCallback<T> tcs = new ResultCallback<T> ();
            async UniTask WaitTimeout ()
            {
                await UniTask.Delay (timeout);
                if (tcs.IsDisposed)
                {
                    return;
                }
                Notify (new T () { State = LemonWaitType.Timeout });
            }

            WaitTimeout ().GetAwaiter ();

            this.tcss.Add (typeof (T), tcs);
            T ret = await tcs.Task.Task;
            return ret;
        }

        public async UniTask<T> Wait<T> (T a, int timeout) where T : struct, IWaitType
        {
            ResultCallback<T> tcs = new ResultCallback<T> ();
            async UniTask WaitTimeout ()
            {
                await UniTask.Delay (timeout);
                if (tcs.IsDisposed)
                {
                    return;
                }
                Notify (new T () { State = LemonWaitType.Timeout });
            }

            WaitTimeout ().GetAwaiter ();

            this.tcss.Add (typeof (T), tcs);
            T ret = await tcs.Task.Task;
            return ret;
        }

        public async UniTask<T> Wait<T> (int timeout, CancellationToken cancellationToken) where T : struct, IWaitType
        {
            ResultCallback<T> tcs = new ResultCallback<T> ();
            async UniTask WaitTimeout ()
            {
                await UniTask.Delay (timeout, cancellationToken: cancellationToken);
                bool retV = cancellationToken.IsCancellationRequested;
                if (retV)
                {
                    return;
                }
                if (tcs.IsDisposed)
                {
                    return;
                }
                Notify (new T () { State = LemonWaitType.Timeout });
            }

            WaitTimeout ().GetAwaiter ();

            this.tcss.Add (typeof (T), tcs);

            void CancelAction ()
            {
                Notify (new T () { State = LemonWaitType.Cancel });
            }

            T ret;
            try
            {
                cancellationToken.Register (CancelAction);
                ret = await tcs.Task.Task;
            }
            finally
            {

            }
            return ret;
        }

        public async UniTask<T> Wait<T> (T a, int timeout, CancellationToken cancellationToken) where T : struct, IWaitType
        {
            ResultCallback<T> tcs = new ResultCallback<T> ();
            async UniTask WaitTimeout ()
            {
                await UniTask.Delay (timeout, cancellationToken: cancellationToken);
                bool retV = cancellationToken.IsCancellationRequested;
                if (retV)
                {
                    return;
                }
                if (tcs.IsDisposed)
                {
                    return;
                }
                Notify (new T () { State = LemonWaitType.Timeout });
            }

            WaitTimeout ().GetAwaiter ();

            this.tcss.Add (typeof (T), tcs);

            void CancelAction ()
            {
                Notify (new T () { State = LemonWaitType.Cancel });
            }

            T ret;
            try
            {
                cancellationToken.Register (CancelAction);
                ret = await tcs.Task.Task;
            }
            finally
            {

            }
            return ret;
        }

        public void Notify<T> (T obj) where T : struct, IWaitType
        {
            Type type = typeof (T);
            if (!this.tcss.TryGetValue (type, out object tcs))
            {
                UnityEngine.Debug.LogError (Utility.Text.Format("type：'{0}'不存在",type));
                return;
            }

            this.tcss.Remove (type);
            ((ResultCallback<T>)tcs).SetResult (obj);
        }
    }
}
