namespace LemonFramework.UIModule
{
    /// <summary>
    /// 交互基类
    /// </summary>
    public abstract class PresenterBase<TView> : IPresenter
    where TView : class, IView
    {
        /// <summary>
        /// 视图
        /// </summary>
        protected TView _view;

        /// <summary>
        /// 视图
        /// </summary>
        public IView View
        {
            get
            {
                return this._view;
            }
            set
            {
                this._view = (TView)(value as TView);
            }
        }

        protected PresenterBase ()
        {
        }

        /// <summary>
        /// 装载
        /// </summary>
        public virtual void Install ()
        {
        }

        /// <summary>
        /// 创建完成
        /// </summary>
        public virtual void OnCreateCompleted ()
        {
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void OnDestroy ()
        {
        }

        /// <summary>
        /// 聚焦
        /// </summary>
        public virtual void OnFocus ()
        {
        }

        /// <summary>
        /// 隐藏完成
        /// </summary>
        public virtual void OnHideCompleted ()
        {
        }

        /// <summary>
        /// 隐藏开始
        /// </summary>
        public virtual void OnHideStart ()
        {
        }

        /// <summary>
        /// 显示完成
        /// </summary>
        public virtual void OnShowCompleted ()
        {
        }

        /// <summary>
        /// 显示开始
        /// </summary>
        public virtual void OnShowStart ()
        {
        }

        /// <summary>
        /// 失焦
        /// </summary>
        public virtual void OnUnFocus ()
        {
        }

        /// <summary>
        /// 卸载
        /// </summary>
        public virtual void Uninstall ()
        {
        }
    }
}