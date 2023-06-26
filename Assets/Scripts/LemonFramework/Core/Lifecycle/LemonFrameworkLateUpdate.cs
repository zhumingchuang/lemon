using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    public static class LemonFrameworkLateUpdate
    {
        public static void AddLateUpdate()
        {

        }

        /// <summary>
        /// 所有框架模块LateUpdate轮询
        /// </summary>
        public static void LateUpdate ()
        {
            foreach (ILemonFrameworkModule module in LemonFrameworkCore.m_FrameworkModule)
            {
                var frameworkLateUpdate = module as ILemonFrameworkLateUpdate;
                if (frameworkLateUpdate != null)
                {
                    frameworkLateUpdate.LateUpdate ();
                }
            }
        }
    }
}
