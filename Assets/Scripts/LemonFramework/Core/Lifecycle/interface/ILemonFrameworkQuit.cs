using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    /// <summary>
    /// 程序退出接口
    /// </summary>
    public interface ILemonFrameworkQuit
    {
        public void OnApplicationQuit ();
    }
}
