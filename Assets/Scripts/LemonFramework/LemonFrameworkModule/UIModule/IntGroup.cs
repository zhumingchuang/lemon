using System;

namespace LemonFramework.UIModule
{
    public struct IntGroup : IEquatable<IntGroup>
    {
        private readonly int[] _ints;

        private readonly static IntGroup _empty;

        public int Count
        {
            get
            {
                int length = 0;
                if (this._ints != null)
                {
                    length = _ints.Length;
                }
                return length;
            }
        }

        public static IntGroup Empty
        {
            get
            {
                return IntGroup._empty;
            }
        }

        public int this[int index]
        {
            get
            {
                int num = -1;
                if (this._ints == null)
                {
                    throw new Exception ("<Ming> ## Uni Exception ## Cls:IntGroup Func:this Info:Null array");
                }
                if (!this._ints.TryGetValue<int> (index, out num))
                {
                    throw new InvalidOperationException ("<Ming> ## Uni Exception ## Cls:IntGroup Func:this Info:Invalid index");
                }
                return num;
            }
        }

        static IntGroup ()
        {
            IntGroup._empty = IntGroup.Get (new int[0]);
        }

        public IntGroup (params int[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException ("<Ming> ## Uni Exception ## Cls:IntGroup Func:Constructor Info:Args is null");
            }
            this._ints = args;
        }

        public bool Contains (int intVal)
        {
            bool flag = false;
            if (this._ints != null)
            {
                int num = 0;
                while (num < this._ints.Length)
                {
                    if (this._ints[num] != intVal)
                    {
                        num++;
                    }
                    else
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }

        public override bool Equals (object obj)
        {
            return this.Equals ((IntGroup)obj);
        }

        public bool Equals (IntGroup other)
        {
            if (this._ints == null || other._ints == null || this._ints.Length != other._ints.Length)
            {
                if (this._ints == null && other._ints == null)
                {
                    return true;
                }
                return false;
            }
            int length = this._ints.Length;
            for (int i = 0; i < length; i++)
            {
                if (this._ints[i] != other._ints[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static IntGroup Get (params int[] args)
        {
            return new IntGroup (args);
        }

        public override int GetHashCode ()
        {
            return base.GetHashCode ();
        }

        public static bool operator == (IntGroup groupA, IntGroup groupB)
        {
            return groupA.Equals (groupB);
        }

        public static bool operator != (IntGroup groupA, IntGroup groupB)
        {
            return !groupA.Equals (groupB);
        }
    }
}