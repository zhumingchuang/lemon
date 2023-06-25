using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    /// <summary>
    /// 特殊堆栈
    /// </summary>
    public class LemonStack<T>
    {
        /// <summary>
        /// 列表
        /// </summary>
        private List<T> _list = new List<T> ();

        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get
            {
                return this._list.Count;
            }
        }

        /// <summary>
        /// 是否包含
        /// </summary>
        public bool Contains (T item)
        {
            return this._list.Contains (item);
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear ()
        {
            this._list.Clear ();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void Delete (T item)
        {
            this._list.Remove (item);
        }

        /// <summary>
        /// 取栈顶
        /// </summary>
        public bool Peek (out T item)
        {
            bool flag = false;
            if (this._list.Count <= 0)
            {
                item = default (T);
            }
            else
            {
                item = this._list[this._list.Count - 1];
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 出栈
        /// </summary>
        public bool Pop (out T item)
        {
            bool flag = false;
            if (this._list.Count <= 0)
            {
                item = default (T);
            }
            else
            {
                item = this._list[this._list.Count - 1];
                this._list.RemoveAt (this._list.Count - 1);
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 入栈
        /// </summary>
        public void Push (T item)
        {
            this._list.Remove (item);
            this._list.Add (item);
        }
    }
}
