using LemonFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    public static class LemonFrameworkFixedUpdate
    {
        internal static readonly LemonLinkedList<ILemonFrameworkFixedUpdate> m_FrameworkFixedUpdate = new LemonLinkedList<ILemonFrameworkFixedUpdate> ();

        /// <summary>
        /// 所有框架模块Fixed轮询
        /// </summary>
        public static void FixedUpdate ()
        {
            foreach (ILemonFrameworkFixedUpdate module in m_FrameworkFixedUpdate)
            {
                module.FixedUpdate ();
            }
        }
    }
}
