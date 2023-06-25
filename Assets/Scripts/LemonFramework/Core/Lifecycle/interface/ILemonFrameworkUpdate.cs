using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    /// <summary>
    /// 框架更新接口
    /// </summary>
    public interface ILemonFrameworkUpdate : ILemonFrameworkModule
    {
        void Update ();
    }
}
