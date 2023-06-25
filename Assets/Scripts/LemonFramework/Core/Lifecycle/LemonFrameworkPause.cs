using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    public static class LemonFrameworkPause
    {
        public static void OnApplicationPause (bool pause)
        {
            foreach (ILemonFrameworkModule module in LemonFrameworkCore.m_FrameworkModule)
            {
                var frameworkPause = module as ILemonFrameworkPause;
                if (frameworkPause != null)
                {
                    frameworkPause.OnApplicationPause (pause);
                }
            }
        }
    }
}
