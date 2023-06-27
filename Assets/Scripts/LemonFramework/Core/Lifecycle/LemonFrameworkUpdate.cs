using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    public static class LemonFrameworkUpdate
    {
        internal static readonly LemonLinkedList<ILemonFrameworkUpdate> m_FrameworkUpdate = new LemonLinkedList<ILemonFrameworkUpdate> ();

        /// <summary>
        /// 所有框架模块轮询
        /// </summary>
        public static void Update ()
        {
            foreach (ILemonFrameworkUpdate module in m_FrameworkUpdate)
            {
                module.Update ();
            }
        }
    }
}
