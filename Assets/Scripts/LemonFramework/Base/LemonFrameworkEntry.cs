using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    /// <summary>
    /// 框架入口
    /// </summary>
    public static class LemonFrameworkEntry
    {
        private static readonly LemonLinkedList<LemonFrameworkModule> m_LemonFrameworkModule = new LemonLinkedList<LemonFrameworkModule> ();
        private static readonly LemonLinkedList<ILemonFrameworkUpdate> m_UpdateFrameworkModule = new LemonLinkedList<ILemonFrameworkUpdate> ();

        /// <summary>
        /// 所有框架模块轮询
        /// </summary>
        public static void Update ()
        {
            foreach (ILemonFrameworkUpdate module in m_UpdateFrameworkModule)
            {
                module.Update ();
            }
        }

        /// <summary>
        /// 关闭并清理所有框架模块
        /// </summary>
        public static void Shutdown ()
        {

        }


        /// <summary>
        /// 创建游戏框架模块。
        /// </summary>
        /// <param name="moduleType">要创建的框架模块类型</param>
        /// <returns>要创建的框架模块</returns>
        private static LemonFrameworkModule CreateModule (Type moduleType)
        {
            LemonFrameworkModule module = (LemonFrameworkModule)Activator.CreateInstance (moduleType);
            if (module == null)
            {
                //throw new LemonFrameworkException (Utility.Text.Format ("Can not create module '{0}'.", moduleType.FullName));
            }

            LinkedListNode<LemonFrameworkModule> current = m_LemonFrameworkModule.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                m_LemonFrameworkModule.AddBefore (current, module);
            }
            else
            {
                m_LemonFrameworkModule.AddLast (module);
            }

            return module;
        }
    }
}
