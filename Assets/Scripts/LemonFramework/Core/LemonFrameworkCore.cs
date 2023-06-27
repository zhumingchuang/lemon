using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace LemonFramework
{
    public static class LemonFrameworkCore
    {
        internal static readonly List<LemonFrameworkModule> m_LemonFrameworkModule = new ();
        internal static readonly LemonLinkedList<ILemonFrameworkModule> m_FrameworkModule = new ();

        private static readonly Dictionary<string, Assembly> m_Assemblies = new ();
        private static readonly Dictionary<string, Type> m_AllTypes = new ();
        private static readonly UnOrderMultiMap<Type, Type> m_Types = new ();
        private static readonly Dictionary<Type, List<object>> m_allEvents =new ();

        /// <summary>
        /// 添加程序集
        /// </summary>
        /// <param name="assembly">程序集</param>
        public static void Add (params Assembly[] assembly)
        {
            List<Type> addTypes = new List<Type> ();
            for (int i = 0; i < assembly.Length; i++)
            {
                m_Assemblies[$"{assembly[i].GetName ().Name}.dll"] = assembly[i];
                addTypes.AddRange (assembly[i].GetTypes ());
            }
            Add (addTypes.ToArray ());
        }

        /// <summary>
        /// 添加程类型
        /// </summary>
        /// <param name="addTypes"></param>
        public static void Add (Type[] addTypes)
        {
            foreach (Type addType in addTypes)
            {
                m_AllTypes[addType.FullName] = addType;
            }

            List<Type> baseAttributeTypes = GetBaseAttributes (addTypes);
            foreach (Type baseAttributeType in baseAttributeTypes)
            {
                foreach (Type type in addTypes)
                {
                    if (type.IsAbstract)
                    {
                        continue;
                    }

                    object[] objects = type.GetCustomAttributes (baseAttributeType, true);
                    if (objects.Length == 0)
                    {
                        continue;
                    }

                    m_Types.Add (baseAttributeType, type);
                }
            }

            foreach (Type type in m_Types[typeof (EventAttribute)])
            {
                IEvent iEvent = Activator.CreateInstance (type) as IEvent;
                if (iEvent != null)
                {
                    Type eventType = iEvent.GetEventType ();
                    if (!m_allEvents.ContainsKey (eventType))
                    {
                        m_allEvents.Add (eventType, new List<object> ());
                    }

                    m_allEvents[eventType].Add (iEvent);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addTypes"></param>
        /// <returns></returns>
        private static List<Type> GetBaseAttributes (Type[] addTypes)
        {
            List<Type> attributeTypes = new List<Type> ();
            foreach (Type type in addTypes)
            {
                if (type.IsAbstract)
                {
                    continue;
                }

                if (type.IsSubclassOf (typeof (BaseAttribute)))
                {
                    attributeTypes.Add (type);
                }
            }

            return attributeTypes;
        }

        /// <summary>
        /// 获取框架模块
        /// </summary>
        /// <typeparam name="T">要获取的框架模块类型</typeparam>
        /// <returns>要获取的框架模块</returns>
        /// <remarks>如果要获取的框架模块不存在，则自动创建该框架模块</remarks>
        public static T GetModule<T> () where T : class
        {
            Type interfaceType = typeof (T);
            if (!interfaceType.IsInterface)
            {
                throw new LemonFrameworkException (Utility.Text.Format ("You must get module by interface, but '{0}' is not.", interfaceType.FullName));
            }

            if (!interfaceType.FullName.StartsWith ("LemonFramework.", StringComparison.Ordinal))
            {
                throw new LemonFrameworkException (Utility.Text.Format ("You must get a Game Framework module, but '{0}' is not.", interfaceType.FullName));
            }

            string moduleName = Utility.Text.Format ("{0}.{1}", interfaceType.Namespace, interfaceType.Name.Substring (1));
            Type moduleType = Type.GetType (moduleName);
            if (moduleType == null)
            {
                throw new LemonFrameworkException (Utility.Text.Format ("Can not find Game Framework module type '{0}'.", moduleName));
            }

            return GetModule (moduleType) as T;
        }

        /// <summary>
        /// 获取框架模块
        /// </summary>
        /// <param name="moduleType">要获取的框架模块类型</param>
        /// <returns>要获取的框架模块</returns>
        /// <remarks>如果要获取的框架模块不存在，则自动创建该框架模块</remarks>
        private static LemonFrameworkModule GetModule (Type moduleType)
        {
            foreach (LemonFrameworkModule module in m_LemonFrameworkModule)
            {
                if (module.GetType () == moduleType)
                {
                    return module;
                }
            }

            return AddModule (moduleType);
        }

        /// <summary>
        /// 创建框架模块
        /// </summary>
        /// <typeparam name="T">要创建的框架模块类型</typeparam>
        /// <returns>要创建的框架模块</returns>
        public static T AddModel<T> () where T : LemonFrameworkModule
        {
            Type type = typeof (T);
            return (T)AddModule (type);
        }

        /// <summary>
        /// 创建框架模块
        /// </summary>
        /// <param name="moduleType">要创建的框架模块类型</param>
        /// <returns>要创建的框架模块</returns>
        public static LemonFrameworkModule AddModule (Type moduleType)
        {
            LemonFrameworkModule module = (LemonFrameworkModule)Activator.CreateInstance (moduleType);
            if (module == null)
            {
                throw new LemonFrameworkException (Utility.Text.Format ("Can not create module '{0}'.", moduleType.FullName));
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
