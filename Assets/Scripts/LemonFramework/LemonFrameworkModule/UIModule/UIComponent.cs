using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework.UIModule
{
    /// <summary>
    /// UI组件
    /// </summary>
    public class UIComponent : LemonFrameworkModule, IPopUIModule
    {
        public static UIComponent Instance;
        private IPopUIModule _module;
        public UIComponent ()
        {
            Instance = this;
            this._module = new PopUIModule ();
        }

        public event Action<int> OnViewEnterCompletedEvent;
        public event Action<int> OnViewEnterStartEvent;
        public event Action<int> OnViewQuitCompletedEvent;
        public event Action<int> OnViewQuitStartEvent;

        public void Enter (int viewId, bool pushStack = true, Action callback = null)
        {
            throw new NotImplementedException ();
        }

        public void Enter (IntGroup viewGroup, bool pushStack = true, Action callback = null)
        {
            throw new NotImplementedException ();
        }

        public bool Pop (Action callback = null)
        {
            throw new NotImplementedException ();
        }

        public void Preload (int viewId)
        {
            throw new NotImplementedException ();
        }

        public void Preload (IntGroup viewGroup)
        {
            throw new NotImplementedException ();
        }

        public void Quit (int viewId, bool leaveStack = false, Action callback = null, bool destroy = false)
        {
            throw new NotImplementedException ();
        }

        public void Quit (IntGroup viewGroup, bool leaveStack = false, Action callback = null, bool destroy = false)
        {
            throw new NotImplementedException ();
        }

        public void QuitAll (bool destroy = false)
        {
            throw new NotImplementedException ();
        }

        public void QuitAll (int stayStackViewId, bool destroy = false)
        {
            throw new NotImplementedException ();
        }

        public void QuitAll (IntGroup stayStackViewGroup, bool destroy = false)
        {
            throw new NotImplementedException ();
        }

        public void QuitOtherAll (IntGroup stayViewGroup, bool destroy = false)
        {
            throw new NotImplementedException ();
        }

        public void QuitOtherAll (int stayViewId, bool destroy = false)
        {
            throw new NotImplementedException ();
        }

        public void ResetStack ()
        {
            throw new NotImplementedException ();
        }

        public void UnFocus (int viewId)
        {
            throw new NotImplementedException ();
        }

        public void UnFocus (IntGroup viewGroup)
        {
            throw new NotImplementedException ();
        }
    }
}
