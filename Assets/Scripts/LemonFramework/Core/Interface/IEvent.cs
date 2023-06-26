using System;
using Cysharp.Threading.Tasks;

namespace LemonFramework
{
    public interface IEvent
    {
        Type GetEventType ();
    }

    public interface IEventClass : IEvent
    {
        void Handle (object a);
    }

    [Event]
    public abstract class AEventClass<A> : IEventClass where A : class
    {
        public Type GetEventType ()
        {
            return typeof (A);
        }

        protected abstract void Run (object a);

        public void Handle (object a)
        {
            try
            {
                Run (a);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError (e);
            }
        }
    }


    [Event]
    public abstract class Event<A> : IEvent where A : struct
    {
        public Type GetEventType ()
        {
            return typeof (A);
        }

        protected abstract void Run ();

        public void Handle ()
        {
            try
            {
                Run ();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError (e);
            }
        }
    }

    [Event]
    public abstract class EventAsync<A> : IEvent where A : struct
    {
        public Type GetEventType ()
        {
            return typeof (A);
        }

        protected abstract UniTask Run ();

        public async UniTask Handle ()
        {
            try
            {
                await Run ();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError (e);
            }
        }
    }



    [Event]
    public abstract class AEvent<A> : IEvent where A : struct
    {
        public Type GetEventType ()
        {
            return typeof (A);
        }

        protected abstract void Run (A a);

        public void Handle (A a)
        {
            try
            {
                Run (a);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError (e);
            }
        }
    }

    [Event]
    public abstract class AEventAsync<A> : IEvent where A : struct
    {
        public Type GetEventType ()
        {
            return typeof (A);
        }

        protected abstract UniTask Run (A a);

        public async UniTask Handle (A a)
        {
            try
            {
                await Run (a);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError (e);
            }
        }
    }
}