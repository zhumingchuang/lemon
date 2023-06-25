using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    public static class LemonFrameworkQuit
    {
        public static void OnApplicationQuit ()
        {
            foreach (ILemonFrameworkModule module in LemonFrameworkCore.m_FrameworkModule)
            {
                var frameworkQuit = module as ILemonFrameworkQuit;
                if (frameworkQuit != null)
                {
                    frameworkQuit.OnApplicationQuit ();
                }
            }
        }
    }
}
