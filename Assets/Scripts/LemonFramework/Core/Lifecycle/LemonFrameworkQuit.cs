using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    public static class LemonFrameworkQuit
    {
        internal static readonly LemonLinkedList<ILemonFrameworkQuit> m_FrameworkQuit = new LemonLinkedList<ILemonFrameworkQuit> ();

        public static void OnApplicationQuit ()
        {
            foreach (ILemonFrameworkQuit module in m_FrameworkQuit)
            {
                module.OnApplicationQuit ();
            }
        }
    }
}
