using System;
using System.Collections.Generic;

namespace LemonFramework.UI
{
    /// <summary>
    /// 弹出UI模块
    /// </summary>
    public sealed class PopUIModule : IPopUIModule
    {
        private IUIModule _uiModule;

        /// <summary>
        /// 视图栈存储需要入栈的视图
        /// </summary>
        private LemonStack<IntGroup> _viewStack = new LemonStack<IntGroup> ();

        /// <summary>
        /// 所有激活过的视图字典
        /// </summary>
        private Dictionary<int, PopUIModule.ViewState> _viewDic = new Dictionary<int, PopUIModule.ViewState> ();

        /// <summary>
        /// 临时需要退出的列表集合
        /// </summary>
        private List<int> _tempQuitList = new List<int> ();

        public PopUIModule ()
        {
            this._uiModule = new UIModule ();
        }

        /// <summary>
        /// 进入视图
        /// </summary>
        /// <param name="viewId">视图id</param>
        /// <param name="pushStack">是否入视图栈</param>
        /// <param name="callback">进入完成回调</param>
        public void Enter (int viewId, bool pushStack = true, Action callback = null)
        {
            PopUIModule.ViewState viewState;
            this._viewDic.TryGetValue (viewId, out viewState);
            if (viewState.active)
            {
                try
                {
                    IUIModule uIModule = this._uiModule;
                    if (uIModule != null)
                    {
                        uIModule.Focus (viewId);
                    }
                }
                catch (Exception exception3)
                {
                    if (exception3 is LemonFrameworkException)
                    {
                        throw;
                    }
                    throw new LemonFrameworkException (Utility.Text.Format ("Enter view {0} Focus with exception '{1}'.", viewId, exception3.ToString ()), exception3);
                }
            }
            else
            {
                try
                {
                    Action<int> onViewSwitchDelegate = this.OnViewEnterStartEvent;
                    if (onViewSwitchDelegate != null)
                    {
                        onViewSwitchDelegate (viewId);
                    }
                }
                catch (Exception exception4)
                {
                    if (exception4 is LemonFrameworkException)
                    {
                        throw;
                    }
                    throw new LemonFrameworkException (Utility.Text.Format ("Enter view {0} start event with exception '{1}'.", viewId, exception4.ToString ()), exception4);
                }
                IUIModule uIModule1 = this._uiModule;
                if (uIModule1 != null)
                {
                    uIModule1.Enter (viewId, () =>
                    {
                        try
                        {
                            Action action = callback;
                            if (action != null)
                            {
                                action ();
                            }
                        }
                        catch (Exception exception)
                        {
                            if (exception is LemonFrameworkException)
                            {
                                throw;
                            }
                            throw new LemonFrameworkException (Utility.Text.Format ("Enter view {0} complete callback  with exception'{1}'.", viewId, exception.ToString ()), exception);
                        }

                        try
                        {
                            IUIModule temp_uiModule = this._uiModule;
                            if (temp_uiModule != null)
                            {
                                temp_uiModule.Focus (viewId);
                            }
                        }
                        catch (Exception exception1)
                        {
                            if (exception1 is LemonFrameworkException)
                            {
                                throw;
                            }
                            throw new LemonFrameworkException (Utility.Text.Format ("Enter view {0} focus with exception '{1}'.", viewId, exception1.ToString ()), exception1);
                        }

                        try
                        {
                            Action<int> onViewEnterCompletedEvent = this.OnViewEnterCompletedEvent;
                            if (onViewEnterCompletedEvent != null)
                            {
                                onViewEnterCompletedEvent (viewId);
                            }
                        }
                        catch (Exception exception2)
                        {
                            if (exception2 is LemonFrameworkException)
                            {
                                throw;
                            }
                            throw new LemonFrameworkException (Utility.Text.Format ("Enter view {0} complete event with exception '{1}'. ", viewId, exception2.ToString ()), exception2);
                        }
                    });
                }
                viewState.active = true;
                this._viewDic[viewId] = viewState;
            }
            if (pushStack)
            {
                this._viewStack.Push (IntGroup.Get (new int[] { viewId }));
            }
        }

        /// <summary>
        /// 进入视图
        /// </summary>
        /// <param name="viewGroup">视图组</param>
        /// <param name="pushStack">是否入视图栈</param>
        /// <param name="callback">完成回调</param>
        public void Enter (IntGroup viewGroup, bool pushStack = true, Action callback = null)
        {
            Action action1 = null;
            int count = viewGroup.Count;
            int num = 0;
            for (int i = 0; i < count; i++)
            {
                int item = viewGroup[i];
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
                this.Enter (item, false, action2);
            }
            if (pushStack)
            {
                this._viewStack.Push (viewGroup);
            }
        }

        /// <summary>
        /// 弹出视图回到上一级视图
        /// </summary>
        /// <param name="callback">完成回调</param>
        /// <returns>是否弹出成功</returns>
        public bool Pop (Action callback = null)
        {
            bool flag = false;
            IntGroup empty = IntGroup.Empty;
            IntGroup intGroup = IntGroup.Empty;
            if (this._viewStack.Count > 1 && this._viewStack.Peek (out empty))
            {
                bool flag1 = false;
                int num = 0;
                while (num < empty.Count)
                {
                    if (!this._viewDic.GetValueAnyway<int, PopUIModule.ViewState> (empty[num]).active)
                    {
                        num++;
                    }
                    else
                    {
                        flag1 = true;
                        break;
                    }
                }
                if (flag1 && this._viewStack.Pop (out empty) && this._viewStack.Peek (out intGroup))
                {
                    flag = true;
                    this.Quit (empty, false, () => this.Enter (intGroup, false, callback), false);
                }
            }
            return flag;
        }

        /// <summary>
        /// 预加载
        /// </summary>
        /// <param name="viewId">视图ID</param>
        public void Preload (int viewId)
        {
            if (!this._viewDic.ContainsKey (viewId))
            {
                this._uiModule.Preload (viewId);
                this._viewDic[viewId] = new PopUIModule.ViewState ()
                {
                    active = false
                };
            }
        }

        /// <summary>
        /// 预加载
        /// </summary>
        /// <param name="viewGroup">视图集合</param>
        public void Preload (IntGroup viewGroup)
        {
            for (int i = 0; i < viewGroup.Count; i++)
            {
                this.Preload (viewGroup[i]);
            }
        }

        /// <summary>
        /// 退出视图
        /// </summary>
        /// <param name="viewId">视图id</param>
        /// <param name="leaveStack">是否出视图栈</param>
        /// <param name="callback">退出完成回调</param>
        /// <param name="destroy">是否销毁视图</param>
        public void Quit (int viewId, bool leaveStack = false, Action callback = null, bool destroy = false)
        {
            PopUIModule.ViewState viewState;
            if (this._viewDic.TryGetValue (viewId, out viewState) && viewState.active)
            {
                try
                {
                    Action<int> onViewSwitchDelegate = this.OnViewQuitStartEvent;
                    if (onViewSwitchDelegate != null)
                    {
                        onViewSwitchDelegate (viewId);
                    }
                }
                catch (Exception exception3)
                {
                    if (exception3 is LemonFrameworkException)
                    {
                        throw;
                    }
                    throw new LemonFrameworkException (Utility.Text.Format ("Quit view {0} event with exception '{1}'.", viewId, exception3.ToString ()), exception3);
                }
                IUIModule uIModule = this._uiModule;
                if (uIModule != null)
                {
                    uIModule.Quit (viewId, () =>
                    {
                        try
                        {
                            Action action = callback;
                            if (action != null)
                            {
                                action ();
                            }
                        }
                        catch (Exception exception)
                        {
                            //Model.Log.Error(exception);
                        }
                        try
                        {
                            IUIModule temp_uiModule = this._uiModule;
                            if (temp_uiModule != null)
                            {
                                temp_uiModule.UnFocus (viewId);
                            }
                        }
                        catch (Exception exception1)
                        {
                            //Model.Log.Error(exception1);
                        }
                        try
                        {
                            Action<int> onViewQuitCompletedEvent = this.OnViewQuitCompletedEvent;
                            if (onViewQuitCompletedEvent != null)
                            {
                                onViewQuitCompletedEvent (viewId);
                            }
                        }
                        catch (Exception exception2)
                        {
                            //Model.Log.Error(exception2);
                        }
                    }, destroy);
                }
                viewState.active = false;
                this._viewDic[viewId] = viewState;
            }
            if (leaveStack)
            {
                this._viewStack.Delete (IntGroup.Get (new int[] { viewId }));
            }
        }

        /// <summary>
        /// 退出视图
        /// </summary>
        /// <param name="viewGroup">视图集合</param>
        /// <param name="leaveStack">是否出视图栈</param>
        /// <param name="callback">退出完成回调</param>
        /// <param name="destroy">是否销毁视图</param>
        public void Quit (IntGroup viewGroup, bool leaveStack = false, Action callback = null, bool destroy = false)
        {
            Action action1 = null;
            int count = viewGroup.Count;
            int num = 0;
            for (int i = 0; i < count; i++)
            {
                int item = viewGroup[i];
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
                this.Quit (item, false, action2, destroy);
            }
            if (leaveStack)
            {
                this._viewStack.Delete (viewGroup);
            }
        }

        /// <summary>
        /// 退出所有视图
        /// </summary>
        /// <param name="destroy">是否销毁</param>
        public void QuitAll (bool destroy = false)
        {
            this._tempQuitList.Clear ();
            foreach (int key in this._viewDic.Keys)
            {
                this._tempQuitList.Add (key);
            }
            for (int i = 0; i < this._tempQuitList.Count; i++)
            {
                this.Quit (this._tempQuitList[i], false, null, destroy);
            }
            this._viewDic.Clear ();
            this.ResetStack ();
        }

        /// <summary>
        /// 退出全部
        /// </summary>
        /// <param name="stayStackViewId">保持在堆栈内的视图id</param>
        /// <param name="destroy">是否销毁</param>
        public void QuitAll (int stayStackViewId, bool destroy = false)
        {
            this._tempQuitList.Clear ();
            foreach (int key in this._viewDic.Keys)
            {
                this._tempQuitList.Add (key);
            }
            for (int i = 0; i < this._tempQuitList.Count; i++)
            {
                int item = this._tempQuitList[i];
                this.Quit (item, item != stayStackViewId, null, destroy);
            }
        }

        /// <summary>
        /// 退出所有视图
        /// </summary>
        /// <param name="stayStackViewGroup">保留在栈内的视图集合</param>
        /// <param name="destroy">是否销毁</param>
        public void QuitAll (IntGroup stayStackViewGroup, bool destroy = false)
        {
            this._tempQuitList.Clear ();
            foreach (int key in this._viewDic.Keys)
            {
                this._tempQuitList.Add (key);
            }
            for (int i = 0; i < this._tempQuitList.Count; i++)
            {
                int item = this._tempQuitList[i];
                bool flag = !stayStackViewGroup.Contains (item);
                this.Quit (item, flag, null, destroy);
            }
        }

        /// <summary>
        /// 退出其他视图组
        /// </summary>
        /// <param name="stayViewGroup">保留的视图集合</param>
        /// <param name="destroy">是否销毁</param>
        public void QuitOtherAll (IntGroup stayViewGroup, bool destroy = false)
        {
            this._tempQuitList.Clear ();
            foreach (int key in this._viewDic.Keys)
            {
                if (stayViewGroup.Contains (key))
                {
                    continue;
                }
                this._tempQuitList.Add (key);
            }
            for (int i = 0; i < this._tempQuitList.Count; i++)
            {
                this.Quit (this._tempQuitList[i], true, null, destroy);
            }
        }

        /// <summary>
        /// 退出其他全部视图
        /// </summary>
        /// <param name="stayViewId">保留的视图</param>
        /// <param name="destroy">是否销毁</param>
        public void QuitOtherAll (int stayViewId, bool destroy = false)
        {
            this._tempQuitList.Clear ();
            foreach (int key in this._viewDic.Keys)
            {
                if (stayViewId == key)
                {
                    continue;
                }
                this._tempQuitList.Add (key);
            }
            for (int i = 0; i < this._tempQuitList.Count; i++)
            {
                this.Quit (this._tempQuitList[i], true, null, destroy);
            }
        }

        /// <summary>
        /// 清空视图栈
        /// </summary>
        public void ResetStack ()
        {
            this._viewStack.Clear ();
        }

        /// <summary>
        /// 取消焦点
        /// </summary>
        /// <param name="viewId">视图id</param>
        public void UnFocus (int viewId)
        {
            try
            {
                IUIModule uIModule = this._uiModule;
                if (uIModule != null)
                {
                    uIModule.UnFocus (viewId);
                }
            }
            catch (Exception exception)
            {
                if (exception is LemonFrameworkException)
                {
                    throw;
                }
                throw new LemonFrameworkException (Utility.Text.Format ("View {0} UnFocus with exception '{1}'.", viewId, exception.ToString ()), exception);
            }
        }

        /// <summary>
        /// 取消焦点
        /// </summary>
        /// <param name="viewGroup">视图集合</param>
        public void UnFocus (IntGroup viewGroup)
        {
            for (int i = 0; i < viewGroup.Count; i++)
            {
                this.UnFocus (viewGroup[i]);
            }
        }

        /// <summary>
        /// 进入视图完成事件
        /// </summary>
        public event Action<int> OnViewEnterCompletedEvent;

        /// <summary>
        /// 进入视图开始事件
        /// </summary>
        public event Action<int> OnViewEnterStartEvent;

        /// <summary>
        /// 退出视图完成事件
        /// </summary>
        public event Action<int> OnViewQuitCompletedEvent;

        /// <summary>
        /// 退出视图开始事件
        /// </summary>
        public event Action<int> OnViewQuitStartEvent;

        /// <summary>
        /// 视图状态
        /// </summary>
        private struct ViewState
        {
            /// <summary>
            /// 激活
            /// </summary>
            public bool active;
        }
    }
}