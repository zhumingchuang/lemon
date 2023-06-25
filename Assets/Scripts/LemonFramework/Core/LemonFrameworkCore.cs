using System;
using System.Collections;
using System.Collections.Generic;

namespace LemonFramework
{
    public static class LemonFrameworkCore
    {
        internal static readonly List<LemonFrameworkModule> m_LemonFrameworkModule = new List<LemonFrameworkModule> ();
        internal static readonly LemonLinkedList<ILemonFrameworkModule> m_FrameworkModule = new LemonLinkedList<ILemonFrameworkModule> ();

        /// <summary>
        /// 获取游戏框架模块
        /// </summary>
        /// <typeparam name="T">要获取的游戏框架模块类型</typeparam>
        /// <returns>要获取的游戏框架模块</returns>
        /// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块</remarks>
        public static T GetModule<T> () where T : class
        {
            Type interfaceType = typeof (T);
            if (!interfaceType.IsInterface)
            {
                throw new LemonFrameworkException (/*Utility.Text.Format ("You must get module by interface, but '{0}' is not.", interfaceType.FullName)*/);
            }

            if (!interfaceType.FullName.StartsWith ("LemonFramework.", StringComparison.Ordinal))
            {
                throw new LemonFrameworkException (/*Utility.Text.Format ("You must get a Game Framework module, but '{0}' is not.", interfaceType.FullName)*/);
            }

            string moduleName = /*Utility.Text.Format ("{0}.{1}", interfaceType.Namespace, interfaceType.Name.Substring (1))*/"";
            Type moduleType = Type.GetType (moduleName);
            if (moduleType == null)
            {
                //throw new LemonFrameworkException (Utility.Text.Format ("Can not find Game Framework module type '{0}'.", moduleName));
            }

            return GetModule (moduleType) as T;
        }

        /// <summary>
        /// 获取游戏框架模块
        /// </summary>
        /// <param name="moduleType">要获取的游戏框架模块类型</param>
        /// <returns>要获取的游戏框架模块</returns>
        /// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块</remarks>
        private static LemonFrameworkModule GetModule (Type moduleType)
        {
            foreach (LemonFrameworkModule module in m_LemonFrameworkModule)
            {
                if (module.GetType () == moduleType)
                {
                    return module;
                }
            }

            return CreateModule (moduleType);
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

            m_LemonFrameworkModule.Add (module);

            return module;
        }

        /// <summary>
        /// 关闭并清理所有框架模块
        /// </summary>
        public static void Shutdown ()
        {

        }
    }
}
