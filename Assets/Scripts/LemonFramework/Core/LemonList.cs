using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    public sealed class LemonList<T> : List<T>, IDisposable
    {
        public static LemonList<T> Create ()
        {
            return LemonPool.Instance.Fetch (typeof (LemonList<T>)) as LemonList<T>;
        }

        public void Dispose ()
        {
            this.Clear ();
            LemonPool.Instance.Recycle (this);
        }
    }
}
