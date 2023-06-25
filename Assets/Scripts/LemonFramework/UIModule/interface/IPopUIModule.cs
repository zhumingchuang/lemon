using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework.UIModule
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPopUIModule
    {
        void Enter (int viewId, bool pushStack = true, Action callback = null);

        void Enter (IntGroup viewGroup, bool pushStack = true, Action callback = null);

        bool Pop (Action callback = null);

        void Preload (int viewId);

        void Preload (IntGroup viewGroup);

        void Quit (int viewId, bool leaveStack = false, Action callback = null, bool destroy = false);

        void Quit (IntGroup viewGroup, bool leaveStack = false, Action callback = null, bool destroy = false);

        void QuitAll (bool destroy = false);

        void QuitAll (int stayStackViewId, bool destroy = false);

        void QuitAll (IntGroup stayStackViewGroup, bool destroy = false);

        void QuitOtherAll (IntGroup stayViewGroup, bool destroy = false);

        void QuitOtherAll (int stayViewId, bool destroy = false);

        void ResetStack ();

        void UnFocus (int viewId);

        void UnFocus (IntGroup viewGroup);

        event Action<int> OnViewEnterCompletedEvent;

        event Action<int> OnViewEnterStartEvent;

        event Action<int> OnViewQuitCompletedEvent;

        event Action<int> OnViewQuitStartEvent;
    }
}
