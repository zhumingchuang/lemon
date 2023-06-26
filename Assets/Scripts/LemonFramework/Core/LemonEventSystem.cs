using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace LemonFramework
{
    using OneTypeSystems = UnOrderMultiMap<Type, object>;

    /// <summary>
    /// 
    /// </summary>
    public sealed class LemonEventSystem : IDisposable
    {
        private readonly Dictionary<string, Type> allTypes = new Dictionary<string, Type> ();

        private readonly UnOrderMultiMap<Type, Type> types = new UnOrderMultiMap<Type, Type> ();

        private TypeSystems typeSystems = new TypeSystems ();


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
        /// <param name="addTypes"></param>
        //public void Add (Type[] addTypes)
        //{
        //    this.allTypes.Clear ();
        //    foreach (Type addType in addTypes)
        //    {
        //        this.allTypes[addType.FullName] = addType;
        //    }

        //    this.types.Clear ();
        //    List<Type> baseAttributeTypes = GetBaseAttributes (addTypes);
        //    foreach (Type baseAttributeType in baseAttributeTypes)
        //    {
        //        foreach (Type type in addTypes)
        //        {
        //            if (type.IsAbstract)
        //            {
        //                continue;
        //            }

        //            object[] objects = type.GetCustomAttributes (baseAttributeType, true);
        //            if (objects.Length == 0)
        //            {
        //                continue;
        //            }

        //            this.types.Add (baseAttributeType, type);
        //        }
        //    }

        //    this.typeSystems = new TypeSystems ();

        //    foreach (Type type in this.GetTypes (typeof (ObjectSystemAttribute)))
        //    {
        //        object obj = Activator.CreateInstance (type);

        //        if (obj is ISystemType iSystemType)
        //        {
        //            OneTypeSystems oneTypeSystems = this.typeSystems.GetOrCreateOneTypeSystems (iSystemType.Type ());
        //            oneTypeSystems.Add (iSystemType.SystemType (), obj);
        //        }
        //    }

        //    this.allEvents.Clear ();
        //    foreach (Type type in types[typeof (EventAttribute)])
        //    {
        //        IEvent iEvent = Activator.CreateInstance (type) as IEvent;
        //        if (iEvent != null)
        //        {
        //            Type eventType = iEvent.GetEventType ();
        //            if (!this.allEvents.ContainsKey (eventType))
        //            {
        //                this.allEvents.Add (eventType, new List<object> ());
        //            }

        //            this.allEvents[eventType].Add (iEvent);
        //        }
        //    }
        //}

        public void Dispose ()
        {

        }

        private sealed class TypeSystems
        {
            private readonly Dictionary<Type, OneTypeSystems> typeSystemsMap = new Dictionary<Type, OneTypeSystems> ();

            public OneTypeSystems GetOrCreateOneTypeSystems (Type type)
            {
                OneTypeSystems systems = null;
                this.typeSystemsMap.TryGetValue (type, out systems);
                if (systems != null)
                {
                    return systems;
                }

                systems = new OneTypeSystems ();
                this.typeSystemsMap.Add (type, systems);
                return systems;
            }

            public OneTypeSystems GetOneTypeSystems (Type type)
            {
                OneTypeSystems systems = null;
                this.typeSystemsMap.TryGetValue (type, out systems);
                return systems;
            }

            public List<object> GetSystems (Type type, Type systemType)
            {
                OneTypeSystems oneTypeSystems = null;
                if (!this.typeSystemsMap.TryGetValue (type, out oneTypeSystems))
                {
                    return null;
                }

                if (!oneTypeSystems.TryGetValue (systemType, out List<object> systems))
                {
                    return null;
                }

                return systems;
            }
        }
    }
}
