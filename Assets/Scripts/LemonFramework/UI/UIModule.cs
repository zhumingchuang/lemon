using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LemonFramework.UI
{
    /// <summary>
    /// UI模块
    /// </summary>
    public class UIModule : IUIModule
    {
        /// <summary>
        /// UI对应字典
        /// </summary>
        private Dictionary<int, IView> _uiDic = new Dictionary<int, IView> ();

        private IView this[int viewId]
        {
            get
            {
                IView view = null;
                this._uiDic.TryGetValue (viewId, out view);
                return view;
            }
            set
            {
                this._uiDic[viewId] = value;
            }
        }

        public UIModule () { }

        /// <summary>
        /// 进入
        /// </summary>
        public void Enter (int viewId, Action callback = null)
        {
            IView item = this[viewId];
            if (item != null)
            {
                item.Show (callback);
                return;
            }
            item = Container.Resolve<IView> (viewId);
            IView view1 = item;
            if (view1 == null)
            {
                return;
            }
            view1.Create (() =>
            {
                this[viewId] = item;
                IView view = item;
                if (view == null)
                {
                    return;
                }
                view.Show (callback);
            });
        }

        /// <summary>
        /// 聚焦
        /// </summary>
        public void Focus (int viewId)
        {
            IView item = this[viewId];
            if (item == null)
            {
                return;
            }
            item.Focus ();
        }

        /// <summary>
        /// 预加载
        /// </summary>
        public void Preload (int viewId)
        {
            IView item = this[viewId];
            if (item == null)
            {
                item = Container.Resolve<IView> (viewId);
                IView view = item;
                if (view == null)
                {
                    return;
                }
                view.Create (() =>
                {
                    this[viewId] = item;
                    item.Active = false;
                });
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void Quit (int viewId, Action callback = null, bool destroy = false)
        {
            IView item = this[viewId];
            if (item != null)
            {
                item.Hide (() =>
                {
                    if (destroy)
                    {
                        item.Destroy ();
                        this._uiDic.Remove (viewId);
                    }
                    Action action = callback;
                    if (action == null)
                    {
                        return;
                    }
                    action ();
                });
            }
        }

        /// <summary>
        /// 退出全部
        /// </summary>
        public void QuitAll (Action callback = null, bool destroy = false)
        {
            Action action1 = null;
            int count = this._uiDic.Count;
            List<int> nums = new List<int> (count);
            foreach (int key in this._uiDic.Keys)
            {
                nums.Add (key);
            }
            int num = 0;
            for (int i = 0; i < count; i++)
            {
                int item = nums[i];
                Action action2 = action1;
                if (action2 == null)
                {
                    Action action3 = () =>
                    {
                        num++;
                        if (num >= count)
                        {
                            Action action = callback;
                            if (action == null)
                            {
                                return;
                            }
                            action ();
                        }
                    };
                    Action action4 = action3;
                    action1 = action3;
                    action2 = action4;
                }
                this.Quit (item, action2, destroy);
            }
        }

        /// <summary>
        /// 失焦
        /// </summary>
        public void UnFocus (int viewId)
        {
            IView item = this[viewId];
            if (item == null)
            {
                return;
            }
            item.UnFocus ();
        }
    }
}