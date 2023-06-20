using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    public class LemonFrameworkModule
    {
        /// <summary>
        /// 获取游戏框架模块优先级
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行</remarks>
        internal virtual int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
