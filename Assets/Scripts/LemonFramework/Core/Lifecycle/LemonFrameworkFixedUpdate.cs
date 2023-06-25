using LemonFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    public static class LemonFrameworkFixedUpdate
    {
        /// <summary>
        /// 所有框架模块Fixed轮询
        /// </summary>
        public static void FixedUpdate ()
        {
            foreach (ILemonFrameworkModule module in LemonFrameworkCore.m_FrameworkModule)
            {
                var frameworkFixedUpdate = module as ILemonFrameworkFixedUpdate;
                if (frameworkFixedUpdate != null)
                {
                    frameworkFixedUpdate.FixedUpdate ();
                }
            }
        }
    }
}
