using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    /// <summary>
    /// 当程序暂停时
    /// </summary>
    public interface ILemonFrameworkPause
    {
        public void OnApplicationPause (bool pause);
    }
}
