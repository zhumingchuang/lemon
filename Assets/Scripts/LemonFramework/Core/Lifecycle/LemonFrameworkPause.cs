using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    public static class LemonFrameworkPause
    {
        internal static readonly LemonLinkedList<ILemonFrameworkPause> m_FrameworkPause = new LemonLinkedList<ILemonFrameworkPause> ();

        public static void OnApplicationPause (bool pause)
        {
            foreach (ILemonFrameworkPause module in m_FrameworkPause)
            {
                module.OnApplicationPause (pause);
            }
        }
    }
}
