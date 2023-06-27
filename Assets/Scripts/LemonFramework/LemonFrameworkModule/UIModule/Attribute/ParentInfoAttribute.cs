using System;

namespace LemonFramework.UIModule
{
    /// <summary>
    /// 父节点信息
    /// </summary>
    [AttributeUsage (AttributeTargets.Class)]
    public class ParentInfoAttribute : Attribute
    {
        /// <summary>
        /// 搜索父节点类型
        /// </summary>
        public FindType type;

        /// <summary>
        /// 参数
        /// </summary>
        public string param = string.Empty;

        public ParentInfoAttribute () { }

        public ParentInfoAttribute (string param)
        {
            this.type = FindType.FindWithName;
            this.param = param;
        }

        public ParentInfoAttribute (int type, string param)
        {
            this.type = (FindType)type;
            this.param = param;
        }
    }
}