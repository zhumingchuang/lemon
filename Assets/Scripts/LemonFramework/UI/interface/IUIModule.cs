using System;

namespace LemonFramework.UI
{
    /// <summary>
    /// UI模块
    /// </summary>
    public interface IUIModule
    {
        /// <summary>
        /// 进入
        /// </summary>
        void Enter (int viewId, Action callback = null);

        /// <summary>
        /// 聚焦
        /// </summary>
        void Focus (int viewId);

        /// <summary>
        /// 预加载
        /// </summary>
        void Preload (int viewId);

        /// <summary>
        /// 退出
        /// </summary>
        void Quit (int viewId, Action callback = null, bool destroy = false);

        /// <summary>
        /// 退出全部
        /// </summary>
        void QuitAll (Action callback = null, bool destroy = false);

        /// <summary>
        /// 失焦
        /// </summary>
        void UnFocus (int viewId);
    }
}
