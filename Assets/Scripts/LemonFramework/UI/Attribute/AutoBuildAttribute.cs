using System;

namespace LemonFramework
{
    /// <summary>
    /// 自动实例化接口
    /// </summary>
    [AttributeUsage (AttributeTargets.Field)]
    public class AutoBuildAttribute : Attribute
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string name;

        public AutoBuildAttribute ()
        {
        }

        public AutoBuildAttribute (string name)
        {
            this.name = name;
        }
    }
}