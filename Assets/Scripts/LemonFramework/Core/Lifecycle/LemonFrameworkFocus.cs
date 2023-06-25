using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    public static class LemonFrameworkFocus
    {
        public static void OnApplicationFocus (bool focus)
        {
            foreach (ILemonFrameworkModule module in LemonFrameworkCore.m_FrameworkModule)
            {
                var frameworkFocus = module as ILemonFrameworkFocus;
                if (frameworkFocus != null)
                {
                    frameworkFocus.OnApplicationFocus (focus);
                }
            }
        }
    }
}
