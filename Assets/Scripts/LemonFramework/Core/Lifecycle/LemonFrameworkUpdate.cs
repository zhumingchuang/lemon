using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    /// <summary>
    /// 框架入口
    /// </summary>
    public static class LemonFrameworkUpdate
    {
        /// <summary>
        /// 所有框架模块轮询
        /// </summary>
        public static void Update ()
        {
            foreach (ILemonFrameworkModule module in LemonFrameworkCore.m_FrameworkModule)
            {
                var frameworkUpdate = module as ILemonFrameworkUpdate;
                if (frameworkUpdate != null)
                {
                    frameworkUpdate.Update ();
                }
            }
        }
    }
}
