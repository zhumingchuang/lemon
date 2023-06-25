using System;

namespace LemonFramework.UIModule
{
    /// <summary>
    /// 资源加载参数
    /// </summary>
    public class ResLoadParam
    {
        private static ResLoadParam _default;

        /// <summary>
        /// 是否为异步
        /// </summary>
        public bool @async;

        /// <summary>
        /// 类型
        /// </summary>
        public Type @type;

        /// <summary>
        /// 默认
        /// </summary>
        public static ResLoadParam Default
        {
            get
            {
                return ResLoadParam._default;
            }
        }

        static ResLoadParam()
        {
            ResLoadParam._default = new ResLoadParam(false, typeof(UnityEngine.GameObject));
        }

        public ResLoadParam(bool async, Type type)
        {
            this.@async = async;
            this.@type = type;
        }
    }
}