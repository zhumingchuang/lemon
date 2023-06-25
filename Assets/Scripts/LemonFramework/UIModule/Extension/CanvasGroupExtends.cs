using UnityEngine;

namespace LemonFramework.UIModule
{
    /// <summary>
    /// CanvasGroup扩展
    /// </summary>
    public static class CanvasGroupExtends
    {
        /// <summary>
        /// 是否激活
        /// </summary>
        public static bool IsActive (this CanvasGroup canvasGroup)
        {
            return canvasGroup.alpha > 0f;
        }

        /// <summary>
        /// 显示隐藏
        /// </summary>
        public static void SetActive (this CanvasGroup canvasGroup, bool active)
        {
            float val;
            CanvasGroup canvasGroup1 = canvasGroup;
            if (active)
            {
                val = 1;
            }
            else
            {
                val = 0;
            }
            canvasGroup1.alpha = val;
            canvasGroup1.interactable = active;
            canvasGroup1.blocksRaycasts = active;
        }
    }
}