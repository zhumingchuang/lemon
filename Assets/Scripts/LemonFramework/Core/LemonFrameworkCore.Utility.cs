using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LemonFramework
{
    public static partial class LemonFrameworkCore
    {
        private static readonly Dictionary<Type, List<object>> m_allEvents = new ();
        private static readonly Dictionary<string, Assembly> m_Assemblies = new ();
        private static readonly UnOrderMultiMap<Type, Type> m_Types = new ();
        private static readonly Dictionary<string, Type> m_AllTypes = new ();

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
        /// 
        /// </summary>
        /// <param name="systemAttributeType"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypes (Type systemAttributeType)
        {
            return m_Types[systemAttributeType];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Type> GetTypes ()
        {
            return m_AllTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <returns></returns>
        public static async UniTask PublishAsync<T> (T a) where T : struct
        {
            if (!m_allEvents.TryGetValue (typeof (T), out var iEvents))
            {
                return;
            }

            using var list = LemonList<UniTask>.Create ();

            for (var i = 0; i < iEvents.Count; ++i)
            {
                var obj = iEvents[i];
                if (obj is not AEventAsync<T> aEvent)
                {
                    UnityEngine.Debug.LogError ($"event error: {obj.GetType ().Name}");
                    continue;
                }

                list.Add (aEvent.Handle (a));
            }

            try
            {
                await UniTask.WhenAll (list.ToArray ());
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError (e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async UniTask PublishAsync<T> () where T : struct
        {
            if (!m_allEvents.TryGetValue (typeof (T), out var iEvents))
            {
                return;
            }

            using var list = LemonList<UniTask>.Create ();

            for (var i = 0; i < iEvents.Count; ++i)
            {
                var obj = iEvents[i];
                if (obj is not EventAsync<T> aEvent)
                {
                    UnityEngine.Debug.LogError ($"event error: {obj.GetType ().Name}");
                    continue;
                }

                list.Add (aEvent.Handle ());
            }

            try
            {
                await UniTask.WhenAll (list.ToArray ());
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError (e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        public static void Publish<T> (T a) where T : struct
        {
            if (!m_allEvents.TryGetValue (typeof (T), out var iEvents))
            {
                return;
            }

            for (var i = 0; i < iEvents.Count; ++i)
            {
                var obj = iEvents[i];
                if (obj is not AEvent<T> aEvent)
                {
                    UnityEngine.Debug.LogError ($"event error: {obj.GetType ().Name}");
                    continue;
                }

                aEvent.Handle (a);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Publish<T> () where T : struct
        {
            if (!m_allEvents.TryGetValue (typeof (T), out var iEvents))
            {
                return;
            }

            for (var i = 0; i < iEvents.Count; ++i)
            {
                var obj = iEvents[i];
                if (obj is not Event<T> aEvent)
                {
                    UnityEngine.Debug.LogError ($"event error: {obj.GetType ().Name}");
                    continue;
                }

                aEvent.Handle ();
            }
        }
    }
}
