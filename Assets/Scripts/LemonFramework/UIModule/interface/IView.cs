using System;

namespace LemonFramework
{
    /// <summary>
    /// 视图接口
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// 激活
        /// </summary>
        bool Active
        {
            get;
            set;
        }

        /// <summary>
        /// 交互
        /// </summary>
        IPresenter Presenter
        {
            get;
            set;
        }

        /// <summary>
        /// 创建
        /// </summary>
        void Create (Action callback = null);

        /// <summary>
        /// 销毁
        /// </summary>
        void Destroy ();

        /// <summary>
        /// 聚焦
        /// </summary>
        void Focus ();

        /// <summary>
        /// 隐藏
        /// </summary>
        void Hide (Action callback = null);

        /// <summary>
        /// 显示
        /// </summary>
        void Show (Action callback = null);

        /// <summary>
        /// 失焦
        /// </summary>
        void UnFocus ();
    }
}