using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    /// <summary>
    /// 当程序失去或获取焦点
    /// </summary>
    public interface ILemonFrameworkFocus
    {
        public void OnApplicationFocus (bool focus);
    }
}
