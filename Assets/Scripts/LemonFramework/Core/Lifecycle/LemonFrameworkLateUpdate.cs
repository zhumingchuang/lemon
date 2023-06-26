using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    public static class LemonFrameworkLateUpdate
    {
        internal static readonly LemonLinkedList<ILemonFrameworkLateUpdate> m_FrameworkLateUpdate = new LemonLinkedList<ILemonFrameworkLateUpdate> ();

        /// <summary>
        /// 所有框架模块LateUpdate轮询
        /// </summary>
        public static void LateUpdate ()
        {
            foreach (ILemonFrameworkLateUpdate module in m_FrameworkLateUpdate)
            {
                module.LateUpdate ();
            }
        }
    }
}
