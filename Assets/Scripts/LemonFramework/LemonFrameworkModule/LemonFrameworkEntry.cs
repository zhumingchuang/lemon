using LemonFramework.UIModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    public static class LemonFrameworkEntry
    {
        public static void Start ()
        {
            LemonFrameworkCore.AddModule (typeof (UIComponent));
        }
    }
}
