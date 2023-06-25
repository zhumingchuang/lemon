using System;
using System.Collections.Generic;

namespace LemonFramework
{
    /// <summary>
    /// 视图组
    /// </summary>
    public class ViewGroup : IView
    {
        protected IPresenter _presenter;

        /// <summary>
        /// 子视图集合
        /// </summary>
        protected List<IView> _subViews = new List<IView> ();

        /// <summary>
        /// 激活
        /// </summary>
        public bool Active
        {
            get
            {
                bool flag = false;
                int num = 0;
                while (num < this._subViews.Count)
                {
                    if (!this._subViews[num].Active)
                    {
                        num++;
                    }
                    else
                    {
                        flag = true;
                        break;
                    }
                }
                return flag;
            }
            set
            {
                for (int i = 0; i < this._subViews.Count; i++)
                {
                    this._subViews[i].Active = value;
                }
            }
        }

        /// <summary>   
        /// 交互控制
        /// </summary>
        public IPresenter Presenter
        {
            get
            {
                return this._presenter;
            }
            set
            {
                if (this._presenter != null)
                {
                    this._presenter.Uninstall ();
                }
                this._presenter = value;
                if (this._presenter != null)
                {
                    this._presenter.View = this;
                    this._presenter.Install ();
                }
            }
        }

        public ViewGroup ()
        {
        }

        public ViewGroup (IView[] subViews)
        {
            this._subViews.AddRange (subViews);
        }

        public void Create (Action callback = null)
        {
            int count = this._subViews.Count;
            int num1 = 0;
            Action action1 = () =>
            {
                int num = num1 + 1;
                num1 = num;
                if (num >= count)
                {
                    IPresenter tempPresenter = this._presenter;
                    if (tempPresenter != null)
                    {
                        tempPresenter.OnCreateCompleted ();
                    }

                    Action action = callback;
                    if (action == null)
                    {
                        return;
                    }
                    action ();
                }
            };
            for (int i = 0; i < this._subViews.Count; i++)
            {
                IView item = this._subViews[i];
                if (item == null)
                {
                    action1 ();
                }
                else
                {
                    item.Create (action1);
                }
            }
        }

        public void Destroy ()
        {
            for (int i = 0; i < this._subViews.Count; i++)
            {
                IView item = this._subViews[i];
                if (item != null)
                {
                    item.Destroy ();
                }
            }
        }

        public void Focus ()
        {
            for (int i = 0; i < this._subViews.Count; i++)
            {
                IView item = this._subViews[i];
                if (item != null)
                {
                    item.Focus ();
                }
            }
        }

        public void Hide (Action callback = null)
        {
            IPresenter presenter = this._presenter;
            if (presenter != null)
            {
                presenter.OnHideStart ();
            }

            int count = this._subViews.Count;
            int num1 = 0;
            Action action1 = () =>
            {
                int num = num1 + 1;
                num1 = num;
                if (num >= count)
                {
                    IPresenter tempPresenter = this._presenter;
                    if (tempPresenter != null)
                    {
                        tempPresenter.OnHideCompleted ();
                    }

                    Action action = callback;
                    if (action == null)
                    {
                        return;
                    }
                    action ();
                }
            };
            for (int i = 0; i < this._subViews.Count; i++)
            {
                IView item = this._subViews[i];
                if (item == null)
                {
                    action1 ();
                }
                else
                {
                    item.Hide (action1);
                }
            }
        }

        public void Show (Action callback = null)
        {
            IPresenter presenter = this._presenter;
            if (presenter != null)
            {
                presenter.OnShowStart ();
            }

            int count = this._subViews.Count;
            int num1 = 0;
            Action action1 = () =>
            {
                int num = num1 + 1;
                num1 = num;
                if (num >= count)
                {
                    IPresenter tempPresenter = this._presenter;
                    if (tempPresenter != null)
                    {
                        tempPresenter.OnShowCompleted ();
                    }
                    Action action = callback;
                    if (action == null)
                    {
                        return;
                    }
                    action ();
                }
            };
            for (int i = 0; i < this._subViews.Count; i++)
            {
                IView item = this._subViews[i];
                if (item == null)
                {
                    action1 ();
                }
                else
                {
                    item.Show (action1);
                }
            }
        }

        public void UnFocus ()
        {
            for (int i = 0; i < this._subViews.Count; i++)
            {
                IView item = this._subViews[i];
                if (item != null)
                {
                    item.UnFocus ();
                }
            }
        }
    }
}