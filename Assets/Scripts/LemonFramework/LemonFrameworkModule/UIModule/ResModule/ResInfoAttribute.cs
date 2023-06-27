using System;

namespace LemonFramework
{
    /// <summary>
    /// 资源特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ResInfoAttribute : Attribute
    {
        /// <summary>
        /// AB包路径
        /// </summary>
        public string abPath;

        /// <summary>
        /// 资源类型
        /// </summary>
        public Type type;

        /// <summary>
        /// 是否异步
        /// </summary>
        public bool @async;

        public ResInfoAttribute()
        {
        }

        public ResInfoAttribute(string abPath)
        {
            this.abPath = abPath;
        }

        public ResInfoAttribute(string abPath, bool async)
        {
            this.abPath = abPath;
            this.@async = async;
        }
    }
}