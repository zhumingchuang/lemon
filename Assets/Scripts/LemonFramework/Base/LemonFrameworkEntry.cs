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
        private static readonly LemonLinkedList<ILemonFrameworkModule> m_FrameworkModule = new LemonLinkedList<ILemonFrameworkModule> ();

        /// <summary>
        /// 所有框架模块轮询
        /// </summary>
        public static void Update ()
        {
            foreach (ILemonFrameworkModule module in m_FrameworkModule)
            {
                var frameworkUpdate = module as ILemonFrameworkUpdate;
                if (frameworkUpdate != null)
                {
                    frameworkUpdate.Update ();
                }
            }
        }

        /// <summary>
        /// 所有框架模块轮询
        /// </summary>
        public static void FixedUpdate ()
        {
            foreach (ILemonFrameworkModule module in m_FrameworkModule)
            {
                var frameworkUpdate = module as ILemonFrameworkFixedUpdate;
                if (frameworkUpdate != null)
                {
                    frameworkUpdate.FixedUpdate ();
                }
            }
        }

        /// <summary>
        /// 关闭并清理所有框架模块
        /// </summary>
        public static void Shutdown ()
        {

        }


        /// <summary>
        /// 创建框架模块
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

            ILemonFrameworkModule lemonFrameworkModule = module as ILemonFrameworkModule;
            if (lemonFrameworkModule != null)
            {
                LinkedListNode<ILemonFrameworkModule> current = m_FrameworkModule.First;
                while (current != null)
                {
                    if (lemonFrameworkModule.Priority > current.Value.Priority)
                    {
                        break;
                    }

                    current = current.Next;
                }

                if (current != null)
                {
                    m_FrameworkModule.AddBefore (current, lemonFrameworkModule);
                }
                else
                {
                    m_FrameworkModule.AddLast (lemonFrameworkModule);
                }
            }

            return module;
        }
    }
}
