using System;
using System.Collections.Generic;
using System.Reflection;

namespace LemonFramework
{
    /// <summary>
    /// IOC容器
    /// </summary>
    public static class Container
    {
        /// <summary>
        /// 类型节点字典 默认16个类型
        /// </summary>
        private readonly static Dictionary<Type, Dictionary<string, Container.ITypeNode>> _dic;

        static Container ()
        {
            Container._dic = new Dictionary<Type, Dictionary<string, Container.ITypeNode>> (16);
        }

        /// <summary>
        /// 构建接口字段
        /// </summary>
        private static void GenerateInterfaceField (object target)
        {
            Type type;
            object obj = target;
            if (target != null)
            {
                type = target.GetType ();
            }
            else
            {
                type = null;
            }
            Container.GenerateInterfaceField (obj, type);
        }

        /// <summary>
        /// 构建接口字段
        /// </summary>
        private static void GenerateInterfaceField (object target, Type type)
        {
            if (target == null || type == null)
            {
                return;
            }
            FieldInfo[] fields = type.GetFields (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (fields != null && fields.Length != 0)
            {
                for (int i = 0; i < (int)fields.Length; i++)
                {
                    FieldInfo fieldInfo = fields[i];
                    if (fieldInfo != null && fieldInfo.FieldType.IsInterface)
                    {
                        object[] customAttributes = fieldInfo.GetCustomAttributes (typeof (AutoBuildAttribute), true);
                        string str = null;
                        if (customAttributes != null && customAttributes.Length != 0)
                        {
                            object[] objArray = customAttributes;
                            int num = 0;
                            while (num < (int)objArray.Length)
                            {
                                AutoBuildAttribute autoBuildAttribute = (AutoBuildAttribute)objArray[num];
                                if (autoBuildAttribute == null)
                                {
                                    num++;
                                }
                                else
                                {
                                    str = autoBuildAttribute.name;
                                    break;
                                }
                            }
                            fieldInfo.SetValue (target, Container.Resolve (fieldInfo.FieldType, str));
                        }
                    }
                }
            }
            Container.GenerateInterfaceField (target, type.BaseType);
        }

        /// <summary>
        /// 注册对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <typeparam name="V">实现</typeparam>
        /// <param name="name">名字</param>
        public static void Regist<T, V> (string name = null)
        where V : class, T, new()
        {
            Container.Regist<T> (name, new Container.NormalTypeNode (typeof (V)));
        }

        /// <summary>
        /// 注册对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <typeparam name="V">实现</typeparam>
        /// <param name="name">名字</param>
        public static void Regist<T, V> (object name)
        where V : class, T, new()
        {
            string str;
            if (name != null)
            {
                str = name.ToString ();
            }
            else
            {
                str = null;
            }
            Container.Regist<T, V> (str);
        }

        /// <summary>
        /// 注册节点
        /// </summary>
        private static void Regist<T> (string name, Container.ITypeNode node)
        {
            name = (string.IsNullOrEmpty (name) ? string.Empty : name);
            Type type = typeof (T);
            Dictionary<string, Container.ITypeNode> strs = null;
            Container._dic.TryGetValue (type, out strs);
            if (strs == null)
            {
                strs = new Dictionary<string, Container.ITypeNode> ();
            }
            strs[name] = node;
            Container._dic[type] = strs;
        }

        /// <summary>
        /// 注册单例对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <typeparam name="V">实现</typeparam>
        /// <param name="obj">已实例化对象</param>
        /// <param name="name">名字</param>
        public static void RegistSingleton<T, V> (V obj, string name)
        where V : class, T, new()
        {
            Container.SingletonTypeNode singletonTypeNode = null;
            singletonTypeNode = (obj == null ? new Container.SingletonTypeNode (typeof (V)) : new Container.SingletonTypeNode ((object)obj));
            Container.Regist<T> (name, singletonTypeNode);
        }

        /// <summary>
        /// 注册单例对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <typeparam name="V">实现</typeparam>
        public static void RegistSingleton<T, V> ()
        where V : class, T, new()
        {
            Container.SingletonTypeNode singletonTypeNode = new Container.SingletonTypeNode (typeof (V));
            Container.Regist<T> (string.Empty, singletonTypeNode);
        }

        /// <summary>
        /// 注册单例对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <typeparam name="V">实现</typeparam>
        /// <param name="name">名字</param>
        public static void RegistSingleton<T, V> (string name)
        where V : class, T, new()
        {
            Container.RegistSingleton<T, V> (default (V), name);
        }

        /// <summary>
        /// 注册单例对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <typeparam name="V">实现</typeparam>
        /// <param name="name">名字</param>
        public static void RegistSingleton<T, V> (object name)
        where V : class, T, new()
        {
            string str;
            if (name != null)
            {
                str = name.ToString ();
            }
            else
            {
                str = null;
            }
            Container.RegistSingleton<T, V> (str);
        }

        /// <summary>
        /// 解析对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <param name="name">名字</param>
        /// <returns>对应实例化对象</returns>
        public static T Resolve<T> (string name = null)
        {
            return (T)Container.Resolve (typeof (T), name);
        }

        /// <summary>
        /// 解析对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <param name="name">名字</param>
        /// <returns>对应实例化对象</returns>
        public static T Resolve<T> (object name)
        {
            string str;
            if (name != null)
            {
                str = name.ToString ();
            }
            else
            {
                str = null;
            }
            return Container.Resolve<T> (str);
        }

        /// <summary>
        /// 解析对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="name">名字</param>
        /// <returns>对应实例化对象</returns>
        public static object Resolve (Type type, string name = null)
        {
            object obj = null;
            name = (string.IsNullOrEmpty (name) ? string.Empty : name);
            Dictionary<string, Container.ITypeNode> strs = null;
            if (Container._dic.TryGetValue (type, out strs))
            {
                Container.ITypeNode typeNode = null;
                if (strs != null)
                {
                    strs.TryGetValue (name, out typeNode);
                }
                if (typeNode is Container.NormalTypeNode)
                {
                    obj = Activator.CreateInstance ((typeNode as Container.NormalTypeNode).objType);
                    Container.GenerateInterfaceField (obj);
                }
                else if (typeNode is Container.SingletonTypeNode)
                {
                    obj = (typeNode as Container.SingletonTypeNode).Obj;
                }
            }
            if (obj == null)
            {
                object[] objArray = new object[] { type, null };
                objArray[1] = (!string.IsNullOrEmpty (name) ? string.Concat (" Name:", name) : string.Empty);
                //Log.Error ("Uni Error Cls:Container Func:Resolve Type:{0}{1} Info:Unregistered", objArray);
                throw new InvalidOperationException ();
            }
            return obj;
        }

        /// <summary>
        /// 清理类型节点
        /// </summary>
        public static void Clear ()
        {
            _dic.Clear ();
        }

        /// <summary>
        /// 类型节点
        /// </summary>
        private interface ITypeNode
        {

        }

        /// <summary>
        /// 简单类型节点
        /// </summary>
        private class NormalTypeNode : Container.ITypeNode
        {
            /// <summary>
            /// 实例对象类型
            /// </summary>
            public Type objType;

            public NormalTypeNode (Type objType)
            {
                this.objType = objType;
            }
        }

        /// <summary>
        /// 单例类型节点
        /// </summary>
        private class SingletonTypeNode : Container.ITypeNode
        {
            private object _obj;

            /// <summary>
            /// 类型
            /// </summary>
            private Type _type;

            /// <summary>
            /// 对象
            /// </summary>
            public object Obj
            {
                get
                {
                    if (this._obj == null)
                    {
                        this._obj = Activator.CreateInstance (this._type);
                        Container.GenerateInterfaceField (this._obj);
                    }
                    return this._obj;
                }
            }

            public SingletonTypeNode (Type objType)
            {
                this._type = objType;
            }

            public SingletonTypeNode (object obj)
            {
                this._obj = obj;
            }
        }
    }
}