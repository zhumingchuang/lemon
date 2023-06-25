using UnityEngine;

namespace LemonFramework.UIModule
{
    public static class LayerNames
    {
        /// <summary>
        /// UI层
        /// </summary>
        public const string UI = "UI";

        /// <summary>
        /// 默认层
        /// </summary>
        public const string DEFAULT = "Default";

        /// <summary>
        /// 隐藏层
        /// </summary>
        public const string HIDDEN = "Hidden";


        public static int GetLayerInt (string name)
        {
            return LayerMask.NameToLayer (name);
        }

        public static string GetLayerStr (int name)
        {
            return LayerMask.LayerToName (name);
        }
    }
}