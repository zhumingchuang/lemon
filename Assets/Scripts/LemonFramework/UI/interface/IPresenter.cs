using System;

namespace LemonFramework
{
    /// <summary> 
    /// 交互
    /// </summary>
    public interface IPresenter
    {
        /// <summary>
        /// 视图
        /// </summary>
        IView View
        {
            get;
            set;
        }

        /// <summary>
        /// 装载
        /// </summary>
        void Install ();

        /// <summary>
        /// 创建完成
        /// </summary>
        void OnCreateCompleted ();

        /// <summary>
        /// 聚焦
        /// </summary>
        void OnFocus ();

        /// <summary>
        /// 隐藏完成
        /// </summary>
        void OnHideCompleted ();

        /// <summary>
        /// 隐藏开始
        /// </summary>
        void OnHideStart ();

        /// <summary>
        /// 显示完成
        /// </summary>
        void OnShowCompleted ();

        /// <summary>
        /// 显示开始
        /// </summary>
        void OnShowStart ();

        /// <summary>
        /// 失焦
        /// </summary>
        void OnUnFocus ();

        /// <summary>
        /// 卸载
        /// </summary>
        void Uninstall ();
    }
}