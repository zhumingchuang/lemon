using LemonFramework;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LemonFramework
{
    public static class StartupExternsion
    {
        public static void LoadLemonFramework (this IStartup startup)
        {

        }

        private static void Loader (byte[] assBytes, byte[] pdbBytes)
        {
           var assembly = Assembly.Load (assBytes, pdbBytes);
            //this.LoadLogic ();
            IStaticMethod start = new MonoStaticMethod (assembly, "LemonFramework.LemonFrameworkEntry", "Start");
            start.Run ();
        }
    }
}
